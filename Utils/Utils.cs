namespace Game.Utils
{
    public static class Utils
    {
        public static bool CanApproveCharacters(List<string> roles)
        {
            if(roles.Contains(Constants.AdministratorRole) || roles.Contains(Constants.HelperRole))
            {
                return true;
            }
            return false;
        }
    }
}
