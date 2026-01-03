using System;
using System.Collections.Generic;

namespace Session_4_Dennis_Hilfinger;

public partial class RegistrationStatus
{
    public byte RegistrationStatusId { get; set; }

    public string RegistrationStatus1 { get; set; } = null!;

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
