﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UIADatabase
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UIAEntities : DbContext
    {
        public UIAEntities()
            : base("name=UIAEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Airport> Airports { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<FareTariff> FareTariffs { get; set; }
        public virtual DbSet<FlightRecord> FlightRecords { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
    }
}
