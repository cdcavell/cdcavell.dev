using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace ClassLibrary.Common.Security
{
    /// <summary>
    /// Class for dynamically building self-signed certificate.
    /// Certificate can be constructed to be valid from one day
    /// through maximum of ten years. Defaults to one day.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 05/14/2023 | User Role Claims Development |~ 
    /// </revision>
    public class SelfSignedCertificate
    {
        private string CertificateName;
        private string Password;
        private int DaysValid;
        private X509Certificate2 Certificate;

        /// <summary>
        /// Constructor Method
        /// </summary>
        /// <method>SelfSignedCertificate()</method>
        public SelfSignedCertificate() : this(0, null, null)
        {
        }

        /// <summary>
        /// Constructor Method
        /// </summary>
        /// <param name="daysValid">int</param>
        /// <method>SelfSignedCertificate(int daysValid)</method>
        public SelfSignedCertificate(int daysValid) : this(daysValid, null, null)
        {
        }

        /// <summary>
        /// Constructor Method
        /// </summary>
        /// <param name="certificateName">string</param>
        /// <method>SelfSignedCertificate(string certificateName)</method>
        public SelfSignedCertificate(string certificateName) : this(0, certificateName, null)
        {
        }

        /// <summary>
        /// Constructor Method
        /// </summary>
        /// <param name="certificateName">string</param>
        /// <param name="password">string</param>
        /// <method>SelfSignedCertificate(string certificateName, string password)</method>
        public SelfSignedCertificate(string certificateName, string password) : this(0, certificateName, password) 
        {
        }

        /// <summary>
        /// Constructor Method
        /// </summary>
        /// <param name="certificateName">string</param>
        /// <param name="daysValid">int</param>
        /// <method>SelfSignedCertificate(int daysValid, string certificateName)</method>
        public SelfSignedCertificate(int daysValid, string certificateName) : this(daysValid, certificateName, null)
        {
        }

        /// <summary>
        /// Constructor Method
        /// </summary>
        /// <param name="daysValid">int</param>
        /// <param name="certificateName">string</param>
        /// <param name="password">string</param>
        /// <method>SelfSignedCertificate(int daysValid, string certificateName, string password)</method>
        public SelfSignedCertificate(int daysValid, string? certificateName = null, string? password = null)
        {           
            if (string.IsNullOrEmpty(certificateName?.Trim()))
                this.CertificateName = $"{this.GetType().Name}.{DateTime.Now.ToString("yyyy.MM.dd.hh.mm.ss")}";
            else
                this.CertificateName = certificateName.Trim();

            if (string.IsNullOrEmpty(password?.Trim()))
            {
                int length = new RandomGenerator().Next(1, 128);
                int numberOfNonAlphanumericCharacters = new RandomGenerator().Next(1, length);
    			this.Password = PasswordStore.GeneratePassword(length, numberOfNonAlphanumericCharacters);
			}
            else
                this.Password = password.Trim();

            if (daysValid < 1)
                this.DaysValid = 1;
            else 
                if (daysValid > 3650)
                    this.DaysValid = 3650;
                else
                    this.DaysValid = daysValid;

            this.Certificate = Build();
        }

        /// <summary>
        /// Method to extract certificate
        /// </summary>
        /// <method>Get()</method>
        /// <returns>X509Certificate2</returns>
        public X509Certificate2 Get()
        {
            return this.Certificate;
        }

        /// <value>string</value>
        public string Name
        {
            get { return this.CertificateName; }
        }

        /// <value>byte[]</value>
        public byte[] PublicKey
        {
            get { return this.Certificate.PublicKey.EncodedKeyValue.RawData; }
        }

        /// <value>string</value>
        public string PublicKeyBase64
        {
            get { return Convert.ToBase64String(this.PublicKey); }
        }

        /// <value>DateTime</value>
        public DateTime EffectiveDate
        {
            get { return DateTime.Parse(this.Certificate.GetEffectiveDateString()); }
        }

        /// <value>DateTime</value>
        public DateTime ExperationDate
        {
            get { return DateTime.Parse(this.Certificate.GetExpirationDateString()); }
        }

        private X509Certificate2 Build()
        {
            SubjectAlternativeNameBuilder sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName("localhost");
            sanBuilder.AddDnsName(Environment.MachineName);

            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN={CertificateName}");

            using (RSA rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));

                request.CertificateExtensions.Add(
                   new X509EnhancedKeyUsageExtension(
                       new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));

                request.CertificateExtensions.Add(sanBuilder.Build());

                var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(this.DaysValid)));

                #if WINDOWS
                    certificate.FriendlyName = CertificateName;
                #endif

                return new X509Certificate2(certificate.Export(X509ContentType.Pfx, this.Password), this.Password, X509KeyStorageFlags.MachineKeySet);
            }
        }
    }
}
