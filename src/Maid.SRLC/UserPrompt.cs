﻿using Sharprompt;

namespace Maid.SRLC
{
    internal sealed class UserPrompt
    {
        public ConsoleColor Theme
        {
            get;
            set;
        } = ConsoleColor.Gray;

        public T PromptSelector<T>(string question,
                                  T[] options,
                                  out int optionIndex,
                                  int defaultValue = 0,
                                  int? selectValue = null)
        {
            int index = 0;
            var result = PromptSelector<T>(question, options, defaultValue, selectValue);
            optionIndex = -1;

            foreach (var item in options)
            {
                if (item is not null)
                {
                    if (item.Equals(result))
                    {
                        optionIndex = index;
                        break;
                    }
                }

                index++;
            }

            if (optionIndex == -1)
                throw new InvalidOperationException();

            return result;
        }

        /// <param name="selectValue">If not null, skip the cInput part and return the value given.</param>
        public T PromptSelector<T>(string question,
                                          T[] options,
                                          int defaultValue = 0,
                                          int? selectValue = null)
        {
            defaultValue = Normalize(defaultValue);

            if (selectValue != null && (selectValue > options.Length || selectValue < 0))
                throw new InvalidOperationException();

            if (defaultValue > options.Length || defaultValue < 0)
                throw new InvalidOperationException();

            Prompt.Symbols.Done = new Symbol("✓", "O");
            Prompt.Symbols.Error = new Symbol("X", "X");
            Prompt.ColorSchema.Answer = Theme;
            Prompt.ColorSchema.Select = Theme;

            if (selectValue != null)
            {
                var result = options.ElementAt((int)selectValue);
                Console.WriteLine($"{question}: {result}");
                return (T)result;
            }
            else
            {
                return Prompt.Select(question, options, defaultValue: options.ElementAt(defaultValue));
            }
        }

        private static int Normalize(int num)
        {
            num--;

            if (num < 0)
                num = 0;

            return num;
        }
    }
}