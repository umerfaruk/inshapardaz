﻿using AutoMapper;
using Inshapardaz.Domain.Entities;

namespace Inshapardaz.Ports.Database
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Dictionary, Entities.Dictionary>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Language, o => o.MapFrom(s => s.Language))
                .ForMember(d => d.IsPublic, o => o.MapFrom(s => s.IsPublic))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.Word, o => o.Ignore())
                .ReverseMap()
                .ForMember(d => d.Downloads, o => o.Ignore());

            CreateMap<Word, Entities.Word>();
            CreateMap<Meaning, Entities.Meaning>();
            CreateMap<Translation, Entities.Translation>();
            CreateMap<WordRelation, Entities.WordRelation>();
        }
    }
}