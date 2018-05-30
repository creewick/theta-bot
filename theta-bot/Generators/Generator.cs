using System;
using System.Collections.Generic;
using System.Linq;
using theta_bot.Classes;

namespace theta_bot.Generators
{
    public abstract class Generator
    {
        protected static bool Depend(Tag[] tags) =>
            tags.Contains(Tag.DependFromStep) || 
            tags.Contains(Tag.DependFromValue);
       
        protected static Tag CodeType(IEnumerable<Tag> tags)
        {
            if (tags.Contains(Tag.Code))
                return Tag.Code;
            if (tags.Contains(Tag.For))
                return Tag.For;
            if (tags.Contains(Tag.While))
                return Tag.While;
            throw new ArgumentException("Generator called without needed tag");
        }
        
        public abstract Exercise Generate(Exercise exercise, Random random, params Tag[] tags);
    }
}