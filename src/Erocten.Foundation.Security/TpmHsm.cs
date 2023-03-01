using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Tpm2Lib;

namespace PowerEasy.Foundation.Security
{
    /// <summary>
    /// TPM 安全类。
    /// https://github.com/Azure/azure-iot-sdk-csharp/blob/main/security/tpm/src/SecurityProviderTpmHsm.cs
    /// </summary>
    public class TpmHsm
    {
        /// <summary>
        /// 获取背书密钥哈希字符串。
        /// </summary>
        /// <returns>背书密钥哈希字符串。</returns>
        public static string GetEndorsementKeyHash()
        {
            var endorsementKey = GetEndorsementKey();
            return endorsementKey == null ? string.Empty : BitConverter.ToString(SHA256.Create().ComputeHash(endorsementKey)).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// 获取背书密钥。
        /// </summary>
        /// <remarks>
        /// 如果您的 TPM 硬件不支持相关的 API 调用，则对 TPM 库的调用可能会返回Tpm2Lib.TssException或Tpm2Lib.TpmException。
        /// </remarks>
        /// <returns>背书密钥。</returns>
        public static byte[] GetEndorsementKey()
        {
            Tpm2Device tpmDevice;
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    tpmDevice = new TbsDevice();
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    tpmDevice = new LinuxTpm2Device();
                }
                else
                {
                    return null;
                }

                tpmDevice.Connect();
            }
            catch (Exception)
            {
                return null;
            }

            var tpm2 = new Tpm2(tpmDevice);
            if (tpmDevice is TcpTpmDevice)
            {
                // 如果我们使用模拟器，我们必须做一些固件通常会做的事情，这些动作必须在连接建立后发生。
                tpmDevice.PowerCycle();
                tpm2.Startup(Su.Clear);
            }

            var ekTemplate = new TpmPublic(
               TpmAlgId.Sha256,
               ObjectAttr.FixedTPM | ObjectAttr.FixedParent | ObjectAttr.SensitiveDataOrigin | ObjectAttr.AdminWithPolicy | ObjectAttr.Restricted | ObjectAttr.Decrypt,
               new byte[]
               {
                    0x83, 0x71, 0x97, 0x67, 0x44, 0x84, 0xb3, 0xf8, 0x1a, 0x90, 0xcc, 0x8d, 0x46, 0xa5, 0xd7, 0x24,
                    0xfd, 0x52, 0xd7, 0x6e, 0x06, 0x52, 0x0b, 0x64, 0xf2, 0xa1, 0xda, 0x1b, 0x33, 0x14, 0x69, 0xaa
               },
               new RsaParms(new SymDefObject(TpmAlgId.Aes, 128, TpmAlgId.Cfb), new NullAsymScheme(), 2048, 0),
               new Tpm2bPublicKeyRsa(new byte[2048 / 8]));

            uint tpm20EkHandle = ((uint)Ht.Persistent << 24) | 0x00010001;

            var persHandle = new TpmHandle(tpm20EkHandle);
            TpmPublic keyPub = tpm2._AllowErrors().ReadPublic(persHandle, out _, out _);

            if (!tpm2._LastCommandSucceeded())
            {
                TpmHandle keyHandle = tpm2.CreatePrimary(
                    new TpmHandle(TpmHandle.RhEndorsement),
                    new SensitiveCreate(),
                    ekTemplate,
                    Array.Empty<byte>(),
                    Array.Empty<PcrSelection>(),
                    out keyPub,
                    out _,
                    out _,
                    creationTicket: out _);
                tpm2.EvictControl(TpmHandle.RhOwner, keyHandle, persHandle);
                tpm2.FlushContext(keyHandle);
            }

            byte[] ek = keyPub.GetTpm2BRepresentation();

            tpm2.Dispose();
            return ek;
        }
    }
}
