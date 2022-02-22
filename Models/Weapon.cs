namespace Game.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public WeaponType WeaponType { get; set; }
        public List<Character> Characters { get; set; } = new List<Character>();
        public ICollection<Perk> Perks { get; set; }
    }

    public enum WeaponType
    {
        Wizard,
        Warrior
    }
}
