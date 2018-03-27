using System.Text;

namespace theta_bot
{
    public static class StringBuilderExtentions
    {
        public static void ShiftLines(this StringBuilder builder, int count)
        {
            var lines = builder.ToString().Split('\n');
            builder = new StringBuilder();
            foreach (var line in lines)
            {
                for (var i = 0; i < count; i++)
                    builder.Append(" ");
                builder.Append(line);
                builder.Append('\n');
            }
        } 
    }
}