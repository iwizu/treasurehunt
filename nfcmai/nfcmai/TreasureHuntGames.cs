//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace nfcmai
{
    using System;
    using System.Collections.Generic;
    
    public partial class TreasureHuntGames
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TreasureHuntGames()
        {
            this.TreasureHuntParticipants = new HashSet<TreasureHuntParticipants>();
            this.TreasureHuntTips = new HashSet<TreasureHuntTips>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> UniqueId { get; set; }
        public Nullable<bool> Finished { get; set; }
        public Nullable<int> Winner { get; set; }
        public Nullable<System.DateTime> Started { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TreasureHuntParticipants> TreasureHuntParticipants { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TreasureHuntTips> TreasureHuntTips { get; set; }
        public virtual TreasureHuntParticipants TreasureHuntParticipants1 { get; set; }
    }
}
