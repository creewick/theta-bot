using System.Text;

namespace theta_bot
{
    public static class StringBuilderExtention
    {
        public static StringBuilder Indent(this StringBuilder builder, int count)
        {
            var lines = builder.ToString().Split('\n');
            builder.Clear();
            foreach (var line in lines)
            {
                if (line.Length == 0) continue;
                builder.Append(new string(' ', count));
                builder.Append(line);
                builder.Append('\n');
            }

            return builder;
        } 
    }
}