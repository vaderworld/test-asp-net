﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWebApp.Data;
using AspNetCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCoreWebApp.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly AspNetCoreWebApp.Data.ApplicationDbContext _context;

        public IndexModel(AspNetCoreWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SelectList? Genres { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? MovieGenre { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Movie == null) return;

            var genres = from m in _context.Movie
                         orderby m.Genre
                         select m.Genre;

            var movies = from m in _context.Movie select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(s => s.Title.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(s => s.Genre ==  MovieGenre);
            }
            
            Genres = new SelectList(await genres.Distinct().ToListAsync());
            Movie = await movies.ToListAsync();
        }
    }
}
