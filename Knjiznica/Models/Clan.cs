using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knjiznica.Models
{
    public class Clan
    {
       
        public int ClanID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string AdresaStanovanja { get; set; }
        public string MjestoStanovanja { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public DateTime DatumUclanjenja = DateTime.Now;

        public Clan(string ime, string prezime, string adresaStanovanja, string email, string telefon)
        {
            this.Ime = ime;
            this.Prezime = prezime;
            this.AdresaStanovanja = adresaStanovanja;
            this.Email = email;
            this.Telefon = telefon;
        }

    }
}