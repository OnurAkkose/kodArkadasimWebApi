//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace kodArkadasim.Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class Fikirler
    {
        public byte Id { get; set; }
        public Nullable<byte> UyeId { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public string Resim { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
    
        public virtual Uyeler Uyeler { get; set; }
    }
}
