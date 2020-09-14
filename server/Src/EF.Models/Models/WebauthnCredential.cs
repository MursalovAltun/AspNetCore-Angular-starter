using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fido2NetLib.Objects;

namespace EF.Models.Models
{
    public class WebauthnCredential
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }
        public virtual PublicKeyCredentialDescriptor Descriptor { get; set; }

        public byte[] PublicKey { get; set; }

        public uint SignatureCounter { get; set; }
    }


    public class PublicKeyCredentialDescriptor
    {
        [Key] public Guid DescriptorId { get; set; }
        public byte[] Id { get; set; }
        public PublicKeyCredentialType Type { get; set; }
        public virtual List<AuthenticatorTransport> Transports { get; set; }
    }

    public class AuthenticatorTransport
    {
        public Guid Id { get; set; }
        public Fido2NetLib.Objects.AuthenticatorTransport Transport { get; set; }
        public Guid PublicKeyCredentialDescriptorId { get; set; }
        public virtual PublicKeyCredentialDescriptor PublicKeyCredentialDescriptor { get; set; }
    }
}