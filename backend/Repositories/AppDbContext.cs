﻿using backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Repositories
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Invites = Set<Invite>();
            Friendships = Set<Friendship>();
        }

        public DbSet<Invite> Invites { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
    }
}
