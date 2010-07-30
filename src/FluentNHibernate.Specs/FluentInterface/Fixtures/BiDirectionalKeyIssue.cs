using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Specs.FluentInterface.Fixtures.BiDirectionalKeyIssue
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
    }

    public class ContactMap : DomainMap<Contact>
    {
        public ContactMap()
        {
            HasMany(x => x.EmailAddresses);
            HasMany(x => x.PhoneNumbers);
        }
    }

    public class Contact : Entity
    {
        public virtual IEnumerable<ContactEmail> EmailAddresses { get; set; }
        public virtual IEnumerable<ContactPhone> PhoneNumbers { get; set; }
    }

    public class ContactEmail : Entity
    {
        public virtual string EmailAddress { get; set; }
        public virtual Contact Contact { get; set; }
    }

    public class ContactPhone : Entity
    {
        public virtual string PhoneNumber { get; set; }
    }

    public class ContactEmailMap : DomainMap<ContactEmail>
    {
        public ContactEmailMap()
        {
            Map(c => c.EmailAddress);
            References(x => x.Contact);
        }
    }

    public class ContactPhoneMap : DomainMap<ContactPhone>
    {
        public ContactPhoneMap()
        {
            Map(c => c.PhoneNumber);
        }
    }

    public class DomainMap<T> : ClassMap<T> where T : Entity
    {
        public DomainMap()
        {
            Id(x => x.Id).Column("id").GeneratedBy.GuidComb();
            Table(typeof(T).Name);
        }
    }

    public class CaseMap : DomainMap<Case>
    {
        public CaseMap()
        {
            References(c => c.AlternateContact);
            References(c => c.Contact);
            Table("Cases");
        }
    }

    public class Case : Entity
    {
        public virtual Contact Contact { get; set; }
        public virtual Contact AlternateContact { get; set; }
    }
}
