using System;

namespace UserRegistration
{
    internal class OfficeEntities : IDisposable
    {
        public object User_Info { get; internal set; }
        public object UserRole { get; internal set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}