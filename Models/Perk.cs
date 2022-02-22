namespace Game.Models
{
    public class Perk
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Increment { get; set; }
        public ICollection<Character>? Characters { get; set; }
        public ICollection<Weapon>? Weapons { get; set; }
    }
}
