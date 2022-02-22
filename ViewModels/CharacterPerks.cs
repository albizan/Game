using Game.Models;

namespace Game.ViewModels
{
    public class CharacterPerks
    {
        public Character? Character { get; set; }
        public ICollection<Perk>? AllPerks { get; set; }
    }
}
