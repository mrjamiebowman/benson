using System;
using System.Collections.Generic;
using System.Text;

namespace BensonCLI.Data.Models
{
    public class Repo
    { 
        public int RepoId { get; set; }

        public string RepoName { get; set; }
        
        public string Path { get; set; }
    }
}
