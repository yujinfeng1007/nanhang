using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace ZHXY.Domain
{
    public class CreateTablesIfNotExist<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext
    {
        private readonly List<CommandModes> _customCommands;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="tablesToValidate">A list of existing table names to validate; null to don't validate table names</param>
        /// <param name="customCommands">A list of custom commands to execute</param>
        public CreateTablesIfNotExist() => _customCommands = OtherMaps.SqlCommands;
        public void InitializeDatabase(TContext context)
        {
            if (_customCommands.Count <= 0)
                return;
            bool dbExists;
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                dbExists = context.Database.Exists();
            }
            if (dbExists)
            {
                var existingTableNames = new List<string>(context.Database.SqlQuery<string>("SELECT table_name FROM INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE'"));

                foreach (var item in _customCommands)
                {
                    bool createTables = false;
                    if (item.TablesToValidate != null && item.TablesToValidate.Length > 0)
                    {
                        //we have some table names to validate
                        createTables = !existingTableNames.Intersect(item.TablesToValidate, StringComparer.InvariantCultureIgnoreCase).Any();
                    }

                    if (createTables)
                    {
                        foreach (var command in item.SqlCommands)
                            context.Database.ExecuteSqlCommand(command);
                    }
                }
            }
            else
            {
                //throw new ApplicationException("No database instance");
            }
        }
    }
}
