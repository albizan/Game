using Microsoft.AspNetCore.Authorization.Infrastructure;
using Game.Utils;

namespace Game.Authorization.AuthorizationHandlers
{
    public static class CharacterOperations
    {
        public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement { Name = Constants.CreateOperationName };
        public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement { Name = Constants.ReadOperationName };
        public static OperationAuthorizationRequirement Update = new OperationAuthorizationRequirement { Name = Constants.UpdateOperationName };
        public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement { Name = Constants.DeleteOperationName };

        public static OperationAuthorizationRequirement ApproveCharacter = new OperationAuthorizationRequirement { Name = Constants.ApproveCharacterOperationName };
    }
}
