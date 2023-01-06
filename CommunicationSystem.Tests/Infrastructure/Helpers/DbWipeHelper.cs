using CommunicationSystem.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommunicationSystem.Tests.Infrastructure.Helpers
{
    public static class DbWipeHelper
    {
        public static void WipeDbData(this DbContext context)
        {
            var tables = context.GetTableNamesInOrderForWipe(excludeTypes: new Type[] { typeof(ContactDto), typeof(ContactMessageDto), typeof(GroupMessageDto)});
            foreach (var tableName in tables)
            {
                var commandString = $"DELETE FROM \"{tableName}\"";
                context.Database.ExecuteSqlRaw(commandString);
            }
        }
        private static IEnumerable<string> GetTableNamesInOrderForWipe(this DbContext context,
             int maxDepth = 10, params Type[] excludeTypes)
        {
            var allEntities = context.Model
                .GetEntityTypes()
                .Where(x => !excludeTypes.Contains(x.ClrType))
                .ToList();

            ThrowExceptionIfCannotWipeSelfRef(allEntities);

            var principalsDict = allEntities
                .SelectMany(x => x.GetForeignKeys()
                    .Select(y => y.PrincipalEntityType)).Distinct()
                .ToDictionary(k => k, v =>
                    v.GetForeignKeys()
                        .Where(y => y.PrincipalEntityType != v)
                        .Select(y => y.PrincipalEntityType).ToList());

            var result = allEntities
                .Where(x => !principalsDict.ContainsKey(x))
                .ToList();
            var reversePrincipals = new List<IEntityType>();
            int depth = 0;
            while (principalsDict.Keys.Any())
            {
                foreach (var principalNoLinks in
                    principalsDict
                        .Where(x => !x.Value.Any()).ToList())
                {
                    reversePrincipals.Add(principalNoLinks.Key);
                    principalsDict
                        .Remove(principalNoLinks.Key);
                    foreach (var removeLink in
                        principalsDict.Where(x =>
                            x.Value.Contains(principalNoLinks.Key)))
                    {
                        removeLink.Value
                            .Remove(principalNoLinks.Key);
                    }
                }
                if (++depth >= maxDepth)
                    ThrowExceptionMaxDepthReached(
                        principalsDict.Keys.ToList(), depth);
            }
            reversePrincipals.Reverse();
            result.AddRange(reversePrincipals);
            return result.Select(x => x.GetTableName());
        }
        private static void ThrowExceptionIfCannotWipeSelfRef(List<IEntityType> allEntities)
        {
            var cannotWipes = allEntities
                .SelectMany(x => x.GetForeignKeys()
                    .Where(y => y.PrincipalEntityType == x
                                && (y.DeleteBehavior == DeleteBehavior.Restrict
                                 || y.DeleteBehavior == DeleteBehavior.ClientSetNull)))
                .ToList();
            if (cannotWipes.Any())
                throw new InvalidOperationException(
                    "You cannot delete all the rows in one go in entity(s): " +
                    string.Join(", ", cannotWipes.Select(x => x.DeclaringEntityType.Name)));
        }

        private static void ThrowExceptionMaxDepthReached(List<IEntityType> principalsDictKeys, int maxDepth)
        {
            throw new InvalidOperationException(
                $"It looked to a depth of {maxDepth} and didn't finish. Possible circular reference?\nentity(s) left: " +
                string.Join(", ", principalsDictKeys.Select(x => x.ClrType.Name)));
        }


    }
}
