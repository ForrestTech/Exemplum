﻿namespace Application.UnitTests.Mapping
{
    using Application.Todo.Models;
    using AutoMapper;
    using Common.Mapping;
    using Domain.Todo;
    using System;
    using System.Runtime.Serialization;
    using TodoList.Models;
    using TodoList.Queries;
    using Xunit;

    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void Should_have_valid_configuration()
        {
            _configuration.AssertConfigurationIsValid();
        }
        
        [Theory]
        [InlineData(typeof(TodoList), typeof(TodoListDto))]
        [InlineData(typeof(TodoItem), typeof(TodoItemDto))]
        public void Should_support_mapping_from_source_to_destination(Type source, Type destination)
        {
            var instance = GetInstanceOf(source);

            _mapper.Map(instance, source, destination);
        }

        private static object GetInstanceOf(Type type)
        {
            return type.GetConstructor(Type.EmptyTypes) != null ? Activator.CreateInstance(type) : FormatterServices.GetUninitializedObject(type);
        }
    }
}