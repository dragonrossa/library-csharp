using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using Knjiznica.Models;
using System.Data;
using System.Web.Configuration;


namespace Knjiznica.Controllers
{
    public class PregledController : Controller
    {

        private List<Zaposlenik> noviZaposlenik = new List<Zaposlenik>();

        private List<Knjiga> ListaKnjiga = new List<Knjiga>();


        private List<Rezervacija> ListaRezervacija = new List<Rezervacija>();


        private string ConnStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // GET: Pregled
        public ActionResult Index()
        {

            return View();
        }


        

              public ActionResult PopisSlobodnihKnjiga()
        {

           

            string freeBooksQuery = "SELECT * FROM slobodne_knjige2";

           

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(freeBooksQuery, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Debug.WriteLine("\t{0}\t{1}\t{2}t{3}\t{4}\t{5}\t{6}",
                            reader[0], reader[1], reader[2], reader[3], reader[4], reader[5], reader[6]);

                        ListaKnjiga.Add(new Knjiga(Int32.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(),
                            reader[3].ToString(), Int32.Parse(reader[4].ToString()), Int32.Parse(reader[5].ToString()), Int32.Parse(reader[5].ToString())));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
            return View(ListaKnjiga);
        }



        public ActionResult PretraziSlobodneKnjige(FormCollection form)
        {
            string slobodnaKnjiga = form["slobodnaKnjiga"];
       


            string freeBooksQuery = "SELECT * FROM slobodne_knjige2 WHERE NaslovKnjige=@slobodnaKnjiga";

            

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(freeBooksQuery, connection);
                command.Parameters.Add("@slobodnaKnjiga", SqlDbType.VarChar, 50).Value = slobodnaKnjiga;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Debug.WriteLine("\t{0}\t{1}\t{2}t{3}\t{4}\t{5}\t{6}",
                            reader[0], reader[1], reader[2], reader[3], reader[4], reader[5], reader[6]);

                        ListaKnjiga.Add(new Knjiga(Int32.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(),
                            reader[3].ToString(), Int32.Parse(reader[4].ToString()), Int32.Parse(reader[5].ToString()), Int32.Parse(reader[5].ToString())));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }




            return View(ListaKnjiga);
        }


        public ActionResult NajpopularnijeKnjige()
        {

           

           
            return View();


        }


        public ActionResult Rezervacija(int id)
        {
            Debug.WriteLine("NUTRA SAM " + id);
            return View();  
        }

        [HttpPost]
        public ActionResult RezervacijaStatus(FormCollection form, int test)
        {
            int knjigaID = test;

            // TODO: Back end validation

            Clan clan = new Clan(form["Ime"], form["Prezime"], form["Adresa"],
                form["Email"], form["Telefon"]);

            Debug.WriteLine("IME: " + clan.Ime);
            Debug.WriteLine("PREZIME: " + clan.Prezime);
            Debug.WriteLine("ADRESA: " + clan.AdresaStanovanja);
            Debug.WriteLine("EMAIL: " + clan.Email);
            Debug.WriteLine("TELEFON: " + clan.Telefon);

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("INSERT INTO Clanovi(Ime, Prezime, AdresaStanovanja, Email, Telefon) output INSERTED.ClanID VALUES (@Ime, @Prezime, @Adresa, @Email, @Telefon)", connection);
                  
                    command.Parameters.Add("@Ime", SqlDbType.VarChar, 50).Value = clan.Ime;
                    command.Parameters.Add("@Prezime", SqlDbType.VarChar, 50).Value = clan.Prezime;
                    command.Parameters.Add("@Adresa", SqlDbType.VarChar, 100).Value = clan.AdresaStanovanja;
                    command.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = clan.Email;
                    command.Parameters.Add("@Telefon", SqlDbType.VarChar, 50).Value = clan.Telefon;

                    int clanID = (int)command.ExecuteScalar();
           
                    Debug.WriteLine(clanID);

                    Debug.WriteLine("Uspješno upisan novi član!");

                    connection.Close();

                    /****************************POSUDBA******************************/

                    Posudba posudba = new Posudba(clanID, knjigaID);

                    Debug.WriteLine(posudba.ClanID);
                    Debug.WriteLine(posudba.KnjigaID);

                    connection.Open();

                    SqlCommand command2 = new SqlCommand("INSERT INTO Posudba(ClanID, KnjigaID) output INSERTED.PosudbaID VALUES (@ClanID, @KnjigaID)", connection);
                   
                    //dodati upis za rezervacijas

                    command2.Parameters.Add("ClanID", SqlDbType.Int, 16).Value = posudba.ClanID;
                    command2.Parameters.Add("KnjigaID", SqlDbType.Int, 16).Value = posudba.KnjigaID;

                    int posudbaId = (int)command2.ExecuteScalar();

                    Debug.WriteLine("Uspješno upisana nova posudba!");

                    connection.Close();

                    /****************************RezervacijaKnjiga******************************/
                    connection.Open();

                    SqlCommand command3 = new SqlCommand("INSERT INTO RezervacijaKnjiga(Ime,Prezime, KnjigaID, PosudbaID) VALUES (@Ime, @Prezime, @KnjigaID, @PosudbaID)", connection);
                  
                    command3.Parameters.Add("@Ime", SqlDbType.VarChar, 50).Value = clan.Ime;
                    command3.Parameters.Add("@Prezime", SqlDbType.VarChar, 50).Value = clan.Prezime;
                    command3.Parameters.Add("@KnjigaID", SqlDbType.Int, 16).Value = posudba.KnjigaID;
                    command3.Parameters.Add("@PosudbaID", SqlDbType.Int, 16).Value = posudbaId;

                    command3.ExecuteNonQuery();

                    Debug.WriteLine("Uspješno upisana nova rezervacija!");

                    connection.Close();


                    /****************************PromjenaUKnjizi******************************/
                    connection.Open();

                    SqlCommand command4 = new SqlCommand("UPDATE Knjige SET BrojPosudba = BrojPosudba + 1 WHERE KnjigaID = @KnjigaID", connection);
                    command4.Parameters.Add("@KnjigaID", SqlDbType.Int, 16).Value = posudba.KnjigaID;
                    command4.ExecuteNonQuery();
                    Debug.WriteLine("Uspješno updejtan broj posudbi knjige!");
                    connection.Close();



                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }








            }





            return View();
        }


        //REGISTRATIONS

       
        public ActionResult Registracija()
        {
   
            return View();
        }


        [HttpPost]
       
        public ActionResult RegistracijaUspjesna(FormCollection form)
        {
            Zaposlenik zaposlenik = new Zaposlenik(form["KorisnickoIme"], form["Lozinka"]);

            Debug.WriteLine("Korisnicko ime: " + zaposlenik.KorisnickoIme);
            Debug.WriteLine("Lozinka: " + zaposlenik.Lozinka);

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                try
                {

                    connection.Open();



                    SqlCommand command = new SqlCommand("INSERT INTO Zaposlenici VALUES (@KorisnickoIme, @Lozinka)", connection);


                    command.Parameters.Add("@KorisnickoIme", SqlDbType.VarChar, 50).Value = zaposlenik.KorisnickoIme;
                    command.Parameters.Add("@Lozinka", SqlDbType.VarChar, 50).Value = zaposlenik.Lozinka;

                    command.ExecuteNonQuery();




                    Debug.WriteLine("Uspješno upisan novi zaposlenik!");

                    connection.Close();




                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }


                return View();
        }


        public ActionResult Prijava()
        {
            return View();
        }

      

        [HttpPost]
        public ActionResult PrijavaUspjesna(FormCollection form)
        {
            string korisnickoIme = form["KorisnickoIme"];
            string lozinka = form["Lozinka"];

            Debug.WriteLine("Korisnicko ime: " + korisnickoIme);
            Debug.WriteLine("Lozinka: " + lozinka);


            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                try
                {

            
                    SqlCommand command = new SqlCommand
                        ("IF EXISTS (SELECT * FROM Zaposlenici WHERE KorisnickoIme = @KorisnickoIme AND Lozinka=@Lozinka) SELECT 1 ELSE SELECT 2", connection);

                    command.Parameters.Add("@KorisnickoIme", SqlDbType.VarChar, 50).Value = korisnickoIme;
                    command.Parameters.Add("@Lozinka", SqlDbType.VarChar, 50).Value = lozinka;



                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Debug.WriteLine("{0}",reader[0]);

                            if (reader[0].ToString() == "1") 
                            {
                                return View("PrijavaUspjesna");
                            }
                            else
                            {
                                return View("PrijavaNeuspjesna");
                            }
                           
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }




                    connection.Close();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

           

            return View();


            
        }


        public ActionResult PrijavaNeuspjesna()
        {
          
            return View();
        }

        public ActionResult PregledRezervacija()
        {

           


            string rezervacijaQuery = "SELECT r.RezervacijaID, r.Ime, r.Prezime, k.NaslovKnjige AS NaslovKnjige, r.PosudbaID FROM RezervacijaKnjiga r JOIN Knjige k ON k.KnjigaID = r.KnjigaID where r.Seminar is NULL and r.Status is null";



            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(rezervacijaQuery, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                   
                        Debug.WriteLine("{0},{1},{2},{3},{4}", reader[0], reader[1], reader[2], reader[3], reader[4]);

                        ListaRezervacija.Add(new Rezervacija
                            (Int32.Parse(reader[0].ToString()), reader[1].ToString(),
                            reader[2].ToString(), reader[3].ToString(), Int32.Parse(reader[4].ToString())));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return View(ListaRezervacija);

        }       

  
        public ActionResult Knjige()
        {

            string freeBooksQuery = "SELECT KnjigaID, NaslovKnjige,Opis, Zatvoren, BrojPosudba,BrojKnjiga FROM Knjige";

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(freeBooksQuery, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Debug.WriteLine("\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                            reader[0], reader[1], reader[2], reader[3], reader[4], reader[5]);

                        ListaKnjiga.Add(new Knjiga(Int32.Parse(reader[0].ToString()),reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), Int32.Parse(reader[4].ToString()), Int32.Parse(reader[5].ToString())));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
           
            return View(ListaKnjiga);
        }


       
       




        public ActionResult IzmijeniKnjigu(int knjigaID, string naslovKnjige, string opis, int brojPosudba, int brojKnjiga)
        {
            Debug.WriteLine("KnjigaID " + knjigaID);
            Debug.WriteLine("Naslov " + naslovKnjige);
            Debug.WriteLine("Opis " + opis);
          
            Debug.WriteLine("Broj pos " + brojPosudba);
            Debug.WriteLine("Broj knjiga" + brojKnjiga);

            ViewBag.knjigaID = knjigaID;
            ViewBag.naslov = naslovKnjige;
            ViewBag.opis = opis;
            ViewBag.brojPosudba = brojPosudba;
            ViewBag.brojKnjiga = brojKnjiga;


            return View();
        }

        public ActionResult IzmijenjenaKnjiga(FormCollection form, int knjigaID)
        {
            
            string naslov = form["naslovKnjige"].ToString();
            string opis = form["opisKnjige"].ToString();
            string brojPosudba = form["brojPosudba"].ToString();
            string brojKnjiga = form["brojKnjiga"].ToString();
            var zatvoreno = Request.Form["Zatvoren"].Split(',');

            string zatvoren = zatvoreno[0].ToString();

            Debug.WriteLine("Ovo je novi naslov knjige " + knjigaID);
            Debug.WriteLine("Ovo je novi naslov knjige " + naslov);
            Debug.WriteLine("Ovo je opis " + opis);
            Debug.WriteLine("Ovo je novi broj posudbi " + brojPosudba);
            Debug.WriteLine("Ovo je novi broj knjiga " + brojKnjiga);
            Debug.WriteLine("Ovo je novi naslov knjiga " + zatvoren);


            //Update knjiga po ID-u kada unesemo neku izmjenu

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                const string updateKnjige = "UPDATE Knjige SET NaslovKnjige = @naslov, Opis = @opis, Zatvoren = @zatvoren, BrojPosudba = @brojPosudba, BrojKnjiga = @brojKnjiga WHERE KnjigaID = @knjigaID";
                SqlCommand command = new SqlCommand(updateKnjige, connection);
                command.Parameters.Add("@knjigaID", SqlDbType.VarChar, 50).Value = knjigaID;
                command.Parameters.Add("@naslov", SqlDbType.VarChar, 50).Value = naslov;
                command.Parameters.Add("@opis", SqlDbType.VarChar, 50).Value = opis;
                command.Parameters.Add("@brojPosudba", SqlDbType.VarChar, 50).Value = brojPosudba;
                command.Parameters.Add("@brojKnjiga", SqlDbType.VarChar, 50).Value = brojKnjiga;
                command.Parameters.Add("@zatvoren", SqlDbType.VarChar, 50).Value = zatvoren;


                try
                {
                    connection.Open();


                    command.ExecuteNonQuery();


                    Debug.WriteLine("Izmijenili smo knjigu!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return View();
        }

   

        public ActionResult IzbrisiKnjigu(string naslov)
        {
            ViewBag.naslov = naslov;

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {

                const string rezervacijaBrisi = "DELETE FROM Knjige WHERE NaslovKnjige = @naslovKnjige";



                SqlCommand command = new SqlCommand(rezervacijaBrisi, connection);


                command.Parameters.Add("@naslovKnjige", SqlDbType.VarChar, 50).Value = naslov;




                try
                {
                    connection.Open();


                    command.ExecuteNonQuery();

                    Debug.WriteLine("Knjiga uspješno izbrisana!");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }



            return View();
        }

  

        public ViewResult UpisKnjige(FormCollection form)
        {
            return View(new Knjiga() { }

            );

        }

        [HttpPost]
        public ViewResult UpisKnjige(Knjiga knjiga)
        {
           
                if (string.IsNullOrEmpty(knjiga.NaslovKnjige))
                {
                    ModelState.AddModelError("NaslovKnjige", "Naslov je obavezan!");
                }


                if (string.IsNullOrEmpty(knjiga.ImePisca))
                {
                    ModelState.AddModelError("ImePisca", "Ime je obavezno!");
                }

                if (string.IsNullOrEmpty(knjiga.PrezimePisca))
                {
                    ModelState.AddModelError("PrezimePisca", "Prezime je obavezno!");
                }

                if (knjiga.GodinaIzdanja == 0)
                {
                    ModelState.AddModelError("GodinaIzdanja", "Godina je obavezna!");
                }

         

                if (knjiga.BrojKnjiga == 0)
                {
                    ModelState.AddModelError("BrojKnjiga", "Broj knjiga je obavezan!");
                }

                knjiga.BrojPosudba = 0;

                if (ModelState.IsValid)
                {


                    //upis nove knjige

                    using (SqlConnection connection = new SqlConnection(ConnStr))
                    {
                        const string novaKnjiga = "INSERT INTO Knjige(NaslovKnjige, ImePisca, PrezimePisca, GodinaIzdanja, Opis, BrojKnjiga, BrojPosudba) VALUES(@naslov, @imePisca, @prezimePisca, @godinaIzdanja, @opis, @brojKnjiga, @brojPosudba)";
                        SqlCommand command = new SqlCommand(novaKnjiga, connection);

                        command.Parameters.Add("@naslov", SqlDbType.VarChar, 50).Value = knjiga.NaslovKnjige;
                        command.Parameters.Add("@imePisca", SqlDbType.VarChar, 50).Value = knjiga.ImePisca;
                        command.Parameters.Add("@prezimePisca", SqlDbType.VarChar, 50).Value = knjiga.PrezimePisca;
                        command.Parameters.Add("@godinaIzdanja", SqlDbType.VarChar, 4).Value = knjiga.GodinaIzdanja;
                        command.Parameters.Add("@opis", SqlDbType.VarChar, 250).Value = knjiga.Opis;
                        command.Parameters.Add("@brojPosudba", SqlDbType.Int, 50).Value = knjiga.BrojPosudba;
                        command.Parameters.Add("@brojKnjiga", SqlDbType.Int, 50).Value = knjiga.BrojKnjiga;



                        try
                        {
                            connection.Open();


                            command.ExecuteNonQuery();


                            Debug.WriteLine("Dodali smo novu knjigu!");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }



                        return View("KnjigaIzdana", knjiga);

                    }
                }
                else
                {
                    return View();
                }

            
        }



            public ActionResult NovaKnjiga(FormCollection form)
        {

            string naslov = form["naslovKnjige"].ToString();
            string imePisca = form["imePisca"].ToString();
            string prezimePisca = form["prezimePisca"].ToString();
            string godinaIzdanja = form["godinaIzdanja"].ToString();
            string opis = form["opisKnjige"].ToString();
            int brojPosudba = 0; 
            string brojKnjiga = form["brojKnjiga"].ToString();


            Debug.WriteLine("Ovo je novi naslov knjige " + naslov);
            Debug.WriteLine("Ovo je novi ime pisca " + imePisca);
            Debug.WriteLine("Ovo je novi ime pisca " + prezimePisca);
            Debug.WriteLine("Ovo je opis " + opis);
            Debug.WriteLine("Ovo je godina " + godinaIzdanja);
            Debug.WriteLine("Ovo je novi broj posudbi " + brojPosudba);
            Debug.WriteLine("Ovo je novi broj knjiga " + brojKnjiga);

 

                //upis nove knjige

                using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                const string novaKnjiga = "INSERT INTO Knjige(NaslovKnjige, ImePisca, PrezimePisca, GodinaIzdanja, Opis, BrojKnjiga, BrojPosudba) VALUES(@naslov, @imePisca, @prezimePisca, @godinaIzdanja, @opis, @brojKnjiga, @brojPosudba)";
                SqlCommand command = new SqlCommand(novaKnjiga, connection);

                    command.Parameters.Add("@naslov", SqlDbType.VarChar, 50).Value = naslov;
                    command.Parameters.Add("@imePisca", SqlDbType.VarChar, 50).Value = imePisca;
                    command.Parameters.Add("@prezimePisca", SqlDbType.VarChar, 50).Value = prezimePisca;
                    command.Parameters.Add("@godinaIzdanja", SqlDbType.VarChar, 4).Value = godinaIzdanja;
                    command.Parameters.Add("@opis", SqlDbType.VarChar, 250).Value = opis;
                    command.Parameters.Add("@brojPosudba", SqlDbType.Int, 50).Value = brojPosudba;
                    command.Parameters.Add("@brojKnjiga", SqlDbType.Int, 50).Value = int.Parse(brojKnjiga);
                


                try
                {
                    connection.Open();


                    command.ExecuteNonQuery();


                    Debug.WriteLine("Dodali smo novu knjigu!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

               

                return View();
           
            
        }



        





        public ActionResult PretraziKnjigu(FormCollection form)
        {
            string naslov = form["naslovKnjige"].ToString();
          
            ViewBag.knjiga = naslov;

            string freeBooksQuery = "SELECT KnjigaID, NaslovKnjige,Opis, Zatvoren, BrojPosudba,BrojKnjiga FROM Knjige WHERE NaslovKnjige=@naslov";
           

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(freeBooksQuery, connection);
                command.Parameters.Add("@naslov", SqlDbType.VarChar, 50).Value = naslov;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Debug.WriteLine("\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                            reader[0], reader[1], reader[2], reader[3], reader[4], reader[5]);

                        ListaKnjiga.Add(new Knjiga(Int32.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), Int32.Parse(reader[4].ToString()), Int32.Parse(reader[5].ToString())));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return View(ListaKnjiga);






            
        }


        public ActionResult Odjava()
        {
            return View();
        }

        public ActionResult Status_rezervacije(int rezervacijaId)
        {
            Debug.WriteLine("NUTRA SAM again " + rezervacijaId);

            Debug.WriteLine("Hej");

            Rezervacija klikanutaRezervacija = new Rezervacija();

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                const string rezervacijaIspis = "SELECT r.RezervacijaID, r.Ime, r.Prezime, k.NaslovKnjige AS NaslovKnjige, r.PosudbaID  FROM RezervacijaKnjiga r JOIN Knjige k ON k.KnjigaID = r.KnjigaID where r.RezervacijaID = @rezervacijaId";
                SqlCommand command = new SqlCommand(rezervacijaIspis, connection);
                command.Parameters.Add("@rezervacijaId", SqlDbType.VarChar, 50).Value = rezervacijaId;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Debug.WriteLine("{0},{1},{2},{3}, {4}", reader[0], reader[1], reader[2], reader[3], reader[4]);

                        klikanutaRezervacija.RezervacijaID = Int32.Parse(reader[0].ToString());
                        klikanutaRezervacija.Ime = reader[1].ToString();
                        klikanutaRezervacija.Prezime = reader[2].ToString();
                        klikanutaRezervacija.NaslovKnjige = reader[3].ToString();
                        klikanutaRezervacija.PosudbaID = Int32.Parse(reader[4].ToString());
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return View(klikanutaRezervacija);
        }


        public ActionResult PopisRezervacija()
        {
        

            string rezervacijaQuery = "SELECT r.RezervacijaID, r.Ime, r.Prezime, k.NaslovKnjige, r.Seminar, r.Status, r.PosudbaID FROM RezervacijaKnjiga r JOIN Knjige k ON k.KnjigaID = r.KnjigaID";

           

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(rezervacijaQuery, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                  
                        Debug.WriteLine("{0},{1},{2},{3},{4},{5}", reader[0], reader[1], reader[2], reader[3], reader[4], reader[5]);

                        ListaRezervacija.Add(new Rezervacija
                            (Int32.Parse(reader[0].ToString()), reader[1].ToString(),
                            reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), Int32.Parse(reader[6].ToString())));



                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return View(ListaRezervacija);


        }


        public ActionResult OdobrenaRezervacija()
        {
            string rezervacijaQuery = "SELECT r.Ime, r.Prezime, k.NaslovKnjige AS NaslovKnjige, r.Seminar, r.Status FROM RezervacijaKnjiga r JOIN Knjige k ON k.KnjigaID = r.KnjigaID where r.Seminar is not NULL and r.Status is not null";

   

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(rezervacijaQuery, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        
                        Debug.WriteLine("{0},{1},{2},{3},{4}", reader[0], reader[1], reader[2], reader[3], reader[4]);

                        ListaRezervacija.Add(new Rezervacija(reader[0].ToString(), reader[1].ToString(),
                            reader[2].ToString(), reader[3].ToString(), reader[4].ToString()));



                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return View(ListaRezervacija);
        }



        public ActionResult NeodobrenaRezervacija()
        {

            string rezervacijaQuery = "SELECT r.RezervacijaID, r.Ime, r.Prezime, k.NaslovKnjige AS NaslovKnjige, r.PosudbaID, r.Seminar, r.Status FROM RezervacijaKnjiga r JOIN Knjige k ON k.KnjigaID = r.KnjigaID where r.Seminar is NULL and r.Status is null";



            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(rezervacijaQuery, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                       
                        Debug.WriteLine("{0},{1},{2},{3},{4}", reader[0], reader[1], reader[2], reader[3], reader[4]);

                        ListaRezervacija.Add(new Rezervacija
                            (Int32.Parse(reader[0].ToString()), reader[1].ToString(),
                            reader[2].ToString(), reader[3].ToString(), Int32.Parse(reader[4].ToString())));



                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return View(ListaRezervacija);
        }

     
        public ActionResult PretraziRezervacije(FormCollection form)
        {
            string kljucnaRijec = form["kljucnaRijec"];
            Debug.WriteLine(kljucnaRijec);
            ViewBag.kljucnaRijec = kljucnaRijec;


            string rezervacijaQuery = "SELECT r.RezervacijaID, r.Ime, r.Prezime, k.NaslovKnjige, r.Seminar, r.Status, r.PosudbaID FROM RezervacijaKnjiga r JOIN Knjige k ON k.KnjigaID = r.KnjigaID where Ime = @kljucnaRijec or Prezime = @kljucnaRijec or NaslovKnjige = @kljucnaRijec or Seminar = @kljucnaRijec or Status = @kljucnaRijec";

    

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(rezervacijaQuery, connection);
                command.Parameters.Add("@kljucnaRijec", SqlDbType.VarChar, 50).Value = kljucnaRijec;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                   
                    while (reader.Read())
                    {
                  
                        Debug.WriteLine("{0},{1},{2},{3},{4},{5}, {6}", reader[0], reader[1], reader[2], reader[3], reader[4], reader[5], reader[6]);

                        ListaRezervacija.Add(new Rezervacija
                            (Int32.Parse(reader[0].ToString()), reader[1].ToString(),
                            reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), Int32.Parse(reader[6].ToString())));



                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return View(ListaRezervacija);






            
        }


        public ActionResult UpisiRezervacijuSaStatusom(FormCollection form, int rezervacijaId)
        {


            string seminar = form["Seminar"].ToString();
            ViewBag.seminar = seminar;

            Session["seminar"] = seminar;

            TempData["seminar"] = "seminar";


            Debug.WriteLine("Ovo je seminar " + seminar);

            string status = form["Status"].ToString();

            ViewBag.status = status;
            Session["status"] = status;

            Debug.WriteLine("Ovo je status " + status);

            Debug.WriteLine("Ovo je id " + rezervacijaId);


            //LATER

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                const string rezervacijaUpdateStatus = "UPDATE RezervacijaKnjiga SET Seminar = @seminar, Status = @status WHERE RezervacijaID = @rezervacijaId";
                SqlCommand command = new SqlCommand(rezervacijaUpdateStatus, connection);
                command.Parameters.Add("@rezervacijaId", SqlDbType.VarChar, 50).Value = rezervacijaId;
                command.Parameters.Add("@seminar", SqlDbType.VarChar, 50).Value = seminar;
                command.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = status;

                try
                {
                    connection.Open();


                    command.ExecuteNonQuery();


                    Debug.WriteLine("Rezervacija je upisana - dodali smo seminar i status!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


   


           

            return View();
        }

        public ActionResult IzmijeniRezervaciju(int rezervacijaId, string ime, string prezime, string naslovKnjige)
        {

        

            Debug.WriteLine("RezervacijaID " + rezervacijaId);
            Debug.WriteLine("Ime " + ime);
            Debug.WriteLine("Prezime " + prezime);
            Debug.WriteLine("NaslovKnjige " + naslovKnjige);

            ViewBag.rezervacija = rezervacijaId;
            ViewBag.ime = ime;
            ViewBag.prezime = prezime;
            ViewBag.naslovKnjige = naslovKnjige;
            ViewBag.seminar = Session["seminar"];
            ViewBag.status = Session["status"];

           


            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                const string NaslovKnjige = "SELECT NaslovKnjige FROM slobodne_knjige2";
               

                SqlCommand command = new SqlCommand(NaslovKnjige, connection);
               


                try
                {


                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Debug.WriteLine("\t{0}",
                            reader[0]);

 

                        ListaKnjiga.Add(new Knjiga(reader[0].ToString()));

                       

                        ViewBag.zamijena = ListaKnjiga.Select(knji => knji.NaslovKnjige).ToArray();

                        string[] knjigice = ListaKnjiga.Select(knji => knji.NaslovKnjige).ToArray();

                        foreach (string knjig in knjigice)
                        {
                           
                            Debug.WriteLine(knjig);


                        }


                    }
                    reader.Close();


                    Debug.WriteLine("Ovo su slobodne knjige!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }




            return View();
        }

        [HttpPost]
        public ActionResult IzmijenjenaRezervacija(int rezervacijaId, string novaKnjiga)
        {

            Debug.WriteLine("RezervacijaID " + rezervacijaId);
            Debug.WriteLine("Nova knjiga " + novaKnjiga);

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                const string zamjenskaKnjigaID = "SELECT KnjigaID FROM free_books WHERE NaslovKnjige=@novaKnjiga";
                SqlCommand command = new SqlCommand(zamjenskaKnjigaID, connection);
                command.Parameters.Add("@novaKnjiga", SqlDbType.VarChar, 50).Value = novaKnjiga;


                try
                {
                    connection.Open();


                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Debug.WriteLine("\t{0}",
                            reader[0]);



                        ViewBag.zamKnjigaID = reader[0].ToString();

                     




                        Debug.WriteLine("Dobili smo knjigaID!");


                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }






            


            //update Posudbe da se makne nova knjiga koja je posuđena + povratak stanje na vidljivo stanje tj free_books

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                const string posudbaIDzaZamjenu = "SELECT PosudbaID from RezervacijaKnjiga where RezervacijaID=@rezervacijaID;";
                SqlCommand command = new SqlCommand(posudbaIDzaZamjenu, connection);
                command.Parameters.Add("@rezervacijaID", SqlDbType.VarChar, 50).Value = rezervacijaId;


                try
                {
                    connection.Open();


                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Debug.WriteLine("\t{0}",
                            reader[0]);



                        ViewBag.zamPosudbaID = reader[0].ToString();





                        Debug.WriteLine("Dobili smo posudbaID!");


                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }



            //update sa zamjenskom knjigom u RezervacijaKnjiga


            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                const string updateNovaKnjiga = "UPDATE RezervacijaKnjiga SET KnjigaID = @zamKnjigaID WHERE RezervacijaID = @rezervacijaID;  UPDATE Posudba SET KnjigaID=@zamKnjigaID where PosudbaID=@zamPosudbaID";
                SqlCommand command = new SqlCommand(updateNovaKnjiga, connection);
                command.Parameters.Add("@zamKnjigaID", SqlDbType.VarChar, 50).Value = ViewBag.zamKnjigaID;
                command.Parameters.Add("@zamPosudbaID", SqlDbType.VarChar, 50).Value = ViewBag.zamPosudbaID;
                command.Parameters.Add("@rezervacijaID", SqlDbType.VarChar, 50).Value = rezervacijaId;


                try
                {
                    connection.Open();

                    command.ExecuteNonQuery();


                    Debug.WriteLine("Uspješno upisana nova knjiga!");
                    Debug.WriteLine("Uspješno upisana nova posudba!");



                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }





            ViewBag.Poruka = "Podaci uneseni!";
            return View();
        }

        public ActionResult IzbrisiRezervaciju(int rezervacijaId, int posudbaID)
        {


            Debug.WriteLine("NUTRA SAM " + rezervacijaId);


             Debug.WriteLine("NUTRA SAM " + posudbaID);

  
            using (SqlConnection connection = new SqlConnection(ConnStr))
            {


           //   DELETE IZ Rezervacije

               const string rezervacijaBrisi = "DELETE FROM RezervacijaKnjiga WHERE RezervacijaID = @rezervacijaId";

              

                SqlCommand command = new SqlCommand(rezervacijaBrisi, connection);
               

                command.Parameters.Add("@rezervacijaId", SqlDbType.Int).Value = rezervacijaId;

           
               

                try
                {
                    connection.Open();


                    command.ExecuteNonQuery();

                    Debug.WriteLine("Rezervacija izbrisana!");
                  
               }
                catch (Exception ex)
               {
                   Console.WriteLine(ex.Message);
               }
            


          
             }

            //DELETE IZ Posudbe



            using (SqlConnection connection2 = new SqlConnection(ConnStr))
            {

                const string posudbaBrisi = "DELETE FROM Posudba WHERE PosudbaID = @posudbaId";


                SqlCommand command2 = new SqlCommand(posudbaBrisi, connection2);

                Debug.WriteLine("NUTRA SAM " + posudbaID);

                command2.Parameters.Add("@posudbaId", SqlDbType.Int).Value = posudbaID;

                try
                {
                    connection2.Open();


                    command2.ExecuteNonQuery();


                    Debug.WriteLine("Knjiga vraćena u Free Books!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }



            return View();
        }


        //testiram


        // GET: Validacije
        public ViewResult IzdavanjeKnjiga(FormCollection form)
        {
            return View(new Knjiga() {}
        
            );

         


        }


        [HttpPost]
        public ViewResult IzdavanjeKnjiga(Knjiga knjiga)
        {
            if (string.IsNullOrEmpty(knjiga.NaslovKnjige))
            {
                ModelState.AddModelError("NaslovKnjige", "Naslov je obavezan!");
            }


            if (string.IsNullOrEmpty(knjiga.ImePisca))
            {
                ModelState.AddModelError("ImePisca", "Ime je obavezno!");
            }

            if (string.IsNullOrEmpty(knjiga.PrezimePisca))
            {
                ModelState.AddModelError("PrezimePisca", "Prezime je obavezno!");
            }

            if (knjiga.GodinaIzdanja==0)
            {
                ModelState.AddModelError("GodinaIzdanja", "Godina je obavezna!");
            }

            if (string.IsNullOrEmpty(knjiga.Opis))
            {
                ModelState.AddModelError("Opis", "Opis je obavezan!");
            }

            if (knjiga.BrojKnjiga==0)
            {
                ModelState.AddModelError("BrojKnjiga", "Broj knjiga je obavezan!");
            }

            knjiga.BrojPosudba = 0;

            if (ModelState.IsValid)
            {


                //upis nove knjige

                using (SqlConnection connection = new SqlConnection(ConnStr))
                {
                    const string novaKnjiga = "INSERT INTO Knjige(NaslovKnjige, ImePisca, PrezimePisca, GodinaIzdanja, Opis, BrojKnjiga, BrojPosudba) VALUES(@naslov, @imePisca, @prezimePisca, @godinaIzdanja, @opis, @brojKnjiga, @brojPosudba)";
                    SqlCommand command = new SqlCommand(novaKnjiga, connection);

                    command.Parameters.Add("@naslov", SqlDbType.VarChar, 50).Value = knjiga.NaslovKnjige;
                    command.Parameters.Add("@imePisca", SqlDbType.VarChar, 50).Value = knjiga.ImePisca;
                    command.Parameters.Add("@prezimePisca", SqlDbType.VarChar, 50).Value = knjiga.PrezimePisca;
                    command.Parameters.Add("@godinaIzdanja", SqlDbType.VarChar, 4).Value = knjiga.GodinaIzdanja;
                    command.Parameters.Add("@opis", SqlDbType.VarChar, 250).Value = knjiga.Opis;
                    command.Parameters.Add("@brojPosudba", SqlDbType.Int, 50).Value = knjiga.BrojPosudba;
                    command.Parameters.Add("@brojKnjiga", SqlDbType.Int, 50).Value = knjiga.BrojKnjiga;



                    try
                    {
                        connection.Open();


                        command.ExecuteNonQuery();


                        Debug.WriteLine("Dodali smo novu knjigu!");
                       
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }



                    return View("KnjigaIzdana", knjiga);

                }
            }
            else
            {
                return View();
            }

        }



        public ActionResult KnjigePoAbecedi()
        {
            string freeBooksQuery = "SELECT KnjigaID,NaslovKnjige FROM Knjige ORDER BY NaslovKnjige ASC";

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(freeBooksQuery, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Debug.WriteLine("\t{0}\t{1}",
                            reader[0], reader[1]);

                        ListaKnjiga.Add(new Knjiga(Int32.Parse(reader[0].ToString()), reader[1].ToString()));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return View(ListaKnjiga);

            //return View();
        }


        public ActionResult UpisNovogClana(FormCollection form)
        {

     

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                try
                {

                    connection.Open();



                    SqlCommand command = new SqlCommand("INSERT INTO Zaposlenici(KorisnickoIme,Lozinka,ImeZaposlenika,PrezimeZaposlenika,AdresaZaposlenika,PostanskiBroj,OdjelZaposlenika)" +
                        "VALUES(@korisnickoIme, @lozinka, @imeZaposlenika, @prezimeZaposlenika, @adresaZaposlenika, @postanskiBroj, @odjelZaposlenika)", connection);


                    command.Parameters.Add("@korisnickoIme", SqlDbType.VarChar, 50).Value = form["korisnickoIme"];
                    command.Parameters.Add("@lozinka", SqlDbType.VarChar, 50).Value = form["lozinka"];
                    command.Parameters.Add("@imeZaposlenika", SqlDbType.VarChar, 50).Value = form["imeZaposlenika"];
                    command.Parameters.Add("@prezimeZaposlenika", SqlDbType.VarChar, 50).Value = form["prezimeZaposlenika"];
                    command.Parameters.Add("@adresaZaposlenika", SqlDbType.VarChar, 50).Value = form["adresaZaposlenika"];
                    command.Parameters.Add("@postanskiBroj", SqlDbType.Int, 50).Value = form["postanskiBroj"];
                    command.Parameters.Add("@odjelZaposlenika", SqlDbType.VarChar, 50).Value = form["odjelZaposlenika"];

                    command.ExecuteNonQuery();



                    Debug.WriteLine("Uspješno upisan novi zaposlenik!");

                    connection.Close();




                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }


            return View();
        }



  
        

        public ActionResult PopisClanova()
        {

           

            string popisClanova = "SELECT ImeZaposlenika, PrezimeZaposlenika, AdresaZaposlenika, PostanskiBroj, OdjelZaposlenika FROM Zaposlenici";


            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(popisClanova, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Debug.WriteLine("\t{0}\t{1}\t{2}t{3}\t{4}",
                            reader[0], reader[1], reader[2], reader[3], reader[4]);

                        noviZaposlenik.Add(new Zaposlenik(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), Int32.Parse(reader[3].ToString()),reader[4].ToString()));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
    
            return View(noviZaposlenik);

      
        }


    }
}