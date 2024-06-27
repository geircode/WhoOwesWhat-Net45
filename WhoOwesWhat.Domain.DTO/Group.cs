using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoOwesWhat.Domain.DTO
{
    public class Group
    {
        public Guid GroupGuid { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedByPersonGuid { get; set; }
    }

    public class SyncGroupModel
    // Dette er en Parameterklasse. Skal kun brukes for å sende data _til_ en funksjon. Andre funksjoner skal ikke returnere denne klassen.
    {
        public Guid GroupGuid { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedByPersonGuid { get; set; }
        public bool IsUsedInPostsOnApp { get; set; }
    }
}
