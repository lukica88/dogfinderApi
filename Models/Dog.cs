using System;
using Image = System.Drawing.Image;

namespace dogfinderapi2.Models
{
    public class Dog
    {
        public string _Id { get; set; }
        [Obsolete("uvek je null")]
        public string id { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string Description { get; set; }
        public string Kind { get; set; }
        public string Found { get; set; }
        public string dateString { get; set; }
        public string Deleted { get; set; }
        public string Vlasnik { get; set; }
        public string Email { get; set; }
        public string Mob { get; set; }
        public string Slika1 { get; set; }
        public string Slika2 { get; set; }
        public string SlikaLosegKvaliteta { get; set; }
        public string Opstina { get; set; }
        public string Grad { get; set; }
        public string Drzava { get; set; }
        public Image Slika1Image { get; set; }
    }
}