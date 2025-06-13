using Aplikacija.Models;

public class PrijavaNaTermin
{
    public int Id { get; set; }

    public string ClanId { get; set; }  // <- Ovdje string, ne int
    public Korisnik Clan { get; set; }

    public int TerminId { get; set; }
    public Termin Termin { get; set; }
}
