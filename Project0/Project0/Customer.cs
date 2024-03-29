﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Project0
{
    public class Customer
    {
        private static readonly int SALT_LENGTH = 32;
        public string Username { get; protected set; }
        private byte[] PasswordHash { get; set; }
        private byte[] Salt { get; set; } = new byte[SALT_LENGTH];
        private readonly List<IAccount> accounts = new List<IAccount>();

        public Customer(string uname, string password)
        {
            Username = uname;
            (new Random()).NextBytes(Salt); //P1TODO Should be done in DB
            PasswordHash = GetHash(password);
        }

        public bool Login(string password)
        {
            return Encoding.UTF8.GetString(PasswordHash) == Encoding.UTF8.GetString(GetHash(password));
        }

        private byte[] GetHash(string s)
        {
            s += Encoding.UTF8.GetString(Salt);
            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
            }
            return hash;
        }

        public int AccountCount()
        {
            return accounts.Count();
        }
        public void AddAccount(IAccount a)
        {
            accounts.Add(a);
        }
        public void RemoveAccount(IAccount a)
        {
            accounts.Remove(a);
        }

        public void RemoveAllAccounts()
        {
            accounts.Clear();
        }
        public string DisplayAllAccounts()
        {
            StringBuilder info = new StringBuilder();
            foreach(IAccount a in accounts)
            {
                info.Append(a.Info());
            }
            return info.ToString();
        }
        
        public bool HasDebt()
        {
            foreach(IAccount a in accounts)
            {
                if (a.Balance < 0) return true;
            }
            return false;
        }

        public bool HasFunds()
        {
            foreach (IAccount a in accounts)
            {
                if (a.Balance > 0) return true;
            }
            return false;
        }

        public List<TAccount> GetAccounts<TAccount>() where TAccount : IAccount
        {
            List<TAccount> list = new List<TAccount>();
            foreach(var account in accounts)
            {
                if (account is TAccount) list.Add((TAccount)account);
            }
            return list;
        }

        public List<string> GetAccountNames<TAccount>()
        {
            return (from a in accounts where a is TAccount select a.Name).ToList();
        }

        public bool HasAccount(string name)
        {
            if (name == null) return false;
            return accounts.Where(a => a.Name == name).Any();
        }

        public IAccount GetAccount(string name)
        {
            return accounts.Where(a => a.Name == name).Single();
        }

    }
}
