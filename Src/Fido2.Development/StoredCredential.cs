#nullable disable

using System.Text.Json.Serialization;
using Fido2NetLib.Objects;

namespace Fido2NetLib.Development;

public class StoredCredential
{
    /// <summary>
    /// The Credential ID of the public key credential source.
    /// </summary>
    public byte[] Id { get; set; }

    /// <summary>
    /// The credential public key of the public key credential source.
    /// </summary>
    public byte[] PublicKey { get; set; }

    /// <summary>
    /// The latest value of the signature counter in the authenticator data from any ceremony using the public key credential source.
    /// </summary>
    public uint SignCount { get; set; }

    /// <summary>
    /// The value returned from getTransports() when the public key credential source was registered.
    /// </summary>
    public AuthenticatorTransport[] Transports { get; set; }

    /// <summary>
    /// The value of the BE flag when the public key credential source was created.
    /// </summary>
    public bool IsBackupEligible { get; set; }

    /// <summary>
    /// The latest value of the BS flag in the authenticator data from any ceremony using the public key credential source.
    /// </summary>
    public bool IsBackedUp { get; set; }

    /// <summary>
    /// The value of the attestationObject attribute when the public key credential source was registered. 
    /// Storing this enables the Relying Party to reference the credential's attestation statement at a later time.
    /// </summary>
    public byte[] AttestationObject { get; set; }

    /// <summary>
    /// The value of the clientDataJSON attribute when the public key credential source was registered. 
    /// Storing this in combination with the above attestationObject item enables the Relying Party to re-verify the attestation signature at a later time.
    /// </summary>
    public byte[] AttestationClientDataJson { get; set; }

    public List<byte[]> DevicePublicKeys { get; set; }

    public byte[] UserId { get; set; }

    public PublicKeyCredentialDescriptor Descriptor { get; set; }

    public byte[] UserHandle { get; set; }

    public string AttestationFormat { get; set; }

    public DateTimeOffset RegDate { get; set; }

    public Guid AaGuid { get; set; }
    /// <summary>
    /// lưu loại xác thực
    /// </summary>
    public string Type { get; set; }
}

public class StoredCredentialJson
{
    /// <summary>
    /// The Credential ID of the public key credential source.
    /// Khi sử dụng JsonConvert.SerializeObject byte sẽ được mã hóa Base64
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The credential public key of the public key credential source.
    /// Khi sử dụng JsonConvert.SerializeObject byte sẽ được mã hóa Base64
    /// </summary>
    public string PublicKey { get; set; }

    /// <summary>
    /// The latest value of the signature counter in the authenticator data from any ceremony using the public key credential source.
    /// </summary>
    public uint SignCount { get; set; }

    /// <summary>
    /// The value returned from getTransports() when the public key credential source was registered.
    /// </summary>
    public AuthenticatorTransport[] Transports { get; set; }

    /// <summary>
    /// The value of the BE flag when the public key credential source was created.
    /// </summary>
    public bool IsBackupEligible { get; set; }

    /// <summary>
    /// The latest value of the BS flag in the authenticator data from any ceremony using the public key credential source.
    /// </summary>
    public bool IsBackedUp { get; set; }

    /// <summary>
    /// The value of the attestationObject attribute when the public key credential source was registered. 
    /// Storing this enables the Relying Party to reference the credential's attestation statement at a later time.
    /// Khi sử dụng JsonConvert.SerializeObject byte sẽ được mã hóa Base64
    /// </summary>
    public string AttestationObject { get; set; }

    /// <summary>
    /// The value of the clientDataJSON attribute when the public key credential source was registered. 
    /// Storing this in combination with the above attestationObject item enables the Relying Party to re-verify the attestation signature at a later time.
    /// Khi sử dụng JsonConvert.SerializeObject byte sẽ được mã hóa Base64
    /// </summary>
    public string AttestationClientDataJson { get; set; }

    /// <summary>
    /// Khi sử dụng JsonConvert.SerializeObject byte sẽ được mã hóa Base64
    /// </summary>
    public List<string> DevicePublicKeys { get; set; }

    /// <summary>
    /// Khi sử dụng JsonConvert.SerializeObject byte sẽ được mã hóa Base64
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// convert về định dạng string 
    /// </summary>
    public object Descriptor { get; set; }

    /// <summary>
    /// Khi sử dụng JsonConvert.SerializeObject byte sẽ được mã hóa Base64
    /// </summary>
    public string UserHandle { get; set; }

    public string AttestationFormat { get; set; }

    public DateTimeOffset RegDate { get; set; }

    public Guid AaGuid { get; set; }
    /// <summary>
    /// lưu loại xác thực
    /// </summary>
    public string Type { get; set; }
}

public class PublicKeyCredentialDescriptorJson
{
    /// <summary>
    /// This member contains the type of the public key credential the caller is referring to.
    /// </summary>
    public int Type { get; }

    /// <summary>
    /// This member contains the credential ID of the public key credential the caller is referring to.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// This OPTIONAL member contains a hint as to how the client might communicate with the managing authenticator of the public key credential the caller is referring to.
    /// </summary>]
    public string Transports { get; }
};

