using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knjiznica.Models
{
    public class Knjiga
    {
        public int KnjigaID { get; set; }
        public string NaslovKnjige { get; set; }
        public string ImePisca { get; set; }
        public string PrezimePisca { get; set; }
        public int GodinaIzdanja { get; set; }
        public int SlobodneKnjige { get; set; }

        //naknadno dodano

        public string Opis { get; set; }
        public string Zatvoren { get; set; }
        public int BrojPosudba { get; set; }
        public int BrojKnjiga { get; set; }
     


        public Knjiga(int knjigaId, string naslovKnjiga, string imePisca, string prezimePisca, int godinaIzdanja, int slobodneKnjige, int brojKnjiga)
        {
            this.KnjigaID = knjigaId;
            this.NaslovKnjige = naslovKnjiga;
            this.ImePisca = imePisca;
            this.PrezimePisca = prezimePisca;
            this.GodinaIzdanja = godinaIzdanja;
            this.SlobodneKnjige = slobodneKnjige;
            this.BrojKnjiga = brojKnjiga;

        }

        public Knjiga(string naslovKnjiga)
        {
            this.NaslovKnjige = naslovKnjiga;
        }


        public Knjiga(int knjigaId, string naslovKnjiga, string opis, string zatvoren, int brojPosudba, int brojKnjiga)
        {
            this.KnjigaID = knjigaId;
            this.NaslovKnjige = naslovKnjiga;
            this.Opis = opis;
            this.Zatvoren = zatvoren;
            this.BrojPosudba = brojPosudba;
            this.BrojKnjiga = brojKnjiga;
        }

        public Knjiga()
        {
        }

        public Knjiga (int knjigaId, string naslovKnjiga)
        {
            this.KnjigaID = knjigaId;
            this.NaslovKnjige = naslovKnjiga;
        }
    }
}