﻿using AutoMapper;

namespace MovicesApi.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MoviesDetailsDto>();
            CreateMap<MovieDto, Movie>()
                 .ForMember(src => src.Poster, opt => opt.Ignore()); 
        }
    }
}
