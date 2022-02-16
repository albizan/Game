namespace Game.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string? OwnerID { get; set; }
        public string? Name { get; set; }
        public int Damage { get; set; }
        public CharacterType Type { get; set; }
        public Weapon? Weapon { get; set; }
        public int? WeaponId { get; set; }
    }

    public enum CharacterType
    {
        Wizard,
        Warrior
    }
}
