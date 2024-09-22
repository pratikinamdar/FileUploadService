using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
namespace FIleUpload.Models
{
    public class FileDb:DbContext
    {

        public DbSet<TextFile> TextFile { get; set; }

        public DbSet<FileStatus> FileStatus {  get; set; }
        public string connstr;


        public FileDb(string connstr)
        {
            this.connstr = connstr;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlServer(connstr);
        }
    }
}
