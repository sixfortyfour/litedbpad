﻿using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NETCOREAPP3_0
namespace LiteDBPad6
#else
namespace LiteDBPad
#endif
{
    public partial class CodeGenerator : IDisposable
    {
        public readonly LiteDBPad.ConnectionProperties ConnectionProperties;
        public readonly string Namespace;
        public readonly string TypeName;

        private LiteDatabase _database = null;
        private IEnumerable<string> _collectionNames;

        public IEnumerable<string> CapitalizedCollectionNames { get; }

        public CodeGenerator(LiteDBPad.ConnectionProperties connectionProperties, string ns, string typeName)
        {
            if (connectionProperties == null)
                throw new ArgumentNullException(nameof(connectionProperties));
            if (string.IsNullOrEmpty(ns))
                throw new ArgumentNullException(nameof(ns));
            if (string.IsNullOrEmpty(typeName))
                throw new ArgumentNullException(nameof(typeName));

            ConnectionProperties = connectionProperties;
            Namespace = ns;
            TypeName = typeName;
            _database = new LiteDatabase(connectionProperties.GetConnectionString());
            _collectionNames = _database.GetCollectionNames();
            CapitalizedCollectionNames = _collectionNames.Select(_ => Capitalize(_));
        }

        private static string Capitalize(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (name.Length < 1)
                return name;

            var ns = new StringBuilder(name);
            ns[0] = char.ToUpper(name[0]);
            return ns.ToString();
        }



        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                _database.Dispose();
                _database = null;

                _disposedValue = true;
            }
        }

        ~CodeGenerator()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
