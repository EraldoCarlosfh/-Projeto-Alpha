using Alpha.Framework.MediatR.EventSourcing.Validators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Alpha.Framework.MediatR.Resources.Extensions
{
    public static class FormatExtensions
    {
        public static string ToFormat(this string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return null;

            var cleared = cnpj.ClearNumberMask();

            if (cleared.Length == 14)
                return string.Format(@"{0:00\.000\.000\/0000\-00}", Convert.ToInt64(cleared));
            else
                return string.Format(@"{0:000\.000\.000\-00}", Convert.ToInt64(cleared));
        }

        public static string ToFormatCNPJ(this string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return null;

            var cleared = cnpj.ClearNumberMask();

            return string.Format(@"{0:00\.000\.000\/0000\-00}", Convert.ToInt64(cleared));
        }

        public static bool IsValidCNPJ(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var cnpj = value.ClearNumberMask();

            if (cnpj == "00000000000000")
                return false;

            const string formato = "6543298765432";
            var digitos = new int[14];
            var soma = new int[2];
            var resultado = new int[2];
            var cnpjValido = new bool[2];

            soma[0] = 0;
            soma[1] = 0;

            resultado[0] = 0;
            resultado[1] = 0;

            cnpjValido[0] = false;
            cnpjValido[1] = false;

            try
            {
                int numeroDigitos;
                for (numeroDigitos = 0; numeroDigitos < 14; numeroDigitos++)
                {
                    digitos[numeroDigitos] = Int32.Parse(cnpj.Substring(numeroDigitos, 1));

                    if (numeroDigitos <= 11)
                        soma[0] += (digitos[numeroDigitos] * Int32.Parse(formato.Substring(numeroDigitos + 1, 1)));

                    if (numeroDigitos <= 12)
                        soma[1] += (digitos[numeroDigitos] * Int32.Parse(formato.Substring(numeroDigitos, 1)));
                }

                for (numeroDigitos = 0; numeroDigitos < 2; numeroDigitos++)
                {
                    resultado[numeroDigitos] = (soma[numeroDigitos] % 11);

                    if ((resultado[numeroDigitos] == 0) || (resultado[numeroDigitos] == 1))
                        cnpjValido[numeroDigitos] = (digitos[12 + numeroDigitos] == 0);
                    else
                        cnpjValido[numeroDigitos] = (digitos[12 + numeroDigitos] == (11 - resultado[numeroDigitos]));
                }
                return (cnpjValido[0] && cnpjValido[1]);
            }
            catch
            {
                return false;
            }
        }

        public static string ToFormatCPF(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var cpf = value.ClearNumberMask();

            return string.Format(@"{0:000\.000\.000\-00}", Convert.ToInt64(cpf));
        }

        public static string HideCPF(this string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return null;

            var clearedCpf = cpf.ClearNumberMask();

            return $"XXX.{clearedCpf.Substring(3, 3)}.{clearedCpf.Substring(6, 3)}-XX";
        }

        public static string HideDocument(this string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return null;

            var clearedCpf = cpf.ClearNumberMask();

            if (clearedCpf.Length == 14)
                return $"{clearedCpf.Substring(0, 2)}XXX.{clearedCpf.Substring(5, 3)}/XXXX-{clearedCpf.Substring(12, 2)}";
            else
                return $"XXX.{clearedCpf.Substring(3, 3)}.{clearedCpf.Substring(6, 3)}-XX";
        }

        public static bool IsValidPhoneNumber(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (value.Length < 10)
                return false;

            var partes = new string[] { value.GetPhoneWithDDD(), value.GetPhoneWithoutDDD() };

            if (partes.Length != 2)
                return false;

            var regEx = new Regex(@"\d+");

            if (!regEx.Match(partes[0]).Success || !regEx.Match(partes[1]).Success)
                return false;

            return (partes[0].Length == 2 && (partes[1].Length == 8 || partes[1].Length == 9));
        }

        public static bool IsValidCPF(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var cpf = value.ClearNumberMask();

            if (cpf.Length != 11)
                return false;

            var igual = true;

            for (var i = 1; i < 11 && igual; i++)
                if (cpf[i] != cpf[0])
                    igual = false;

            if (igual || cpf == "12345678909" || cpf == "00000000000")
                return false;

            var numeros = new int[11];

            for (var i = 0; i < 11; i++)
                numeros[i] = Int32.Parse(cpf[i].ToString());

            var soma = 0;

            for (var i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            var resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;

            for (var i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else if (numeros[10] != 11 - resultado)
                return false;

            return true;
        }

        public static string ToFormatCEP(this string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                return null;

            if (cep.Contains("-"))
                return cep;

            return cep.Insert(5, "-");
        }

        public static bool IsValidCEP(this string value)
        {
            var clearedCEP = value.ClearNumberMask();

            var regex = new Regex("^[0-9]{8}$");
            return regex.IsMatch(clearedCEP);
        }

        public static string ToFormatVehiclePlate(this string plate)
        {
            if (string.IsNullOrWhiteSpace(plate))
                return null;

            var clearedPlate = ClearVehiclePlate(plate);

            return clearedPlate.Insert(3, "-");
        }

        public static string ClearVehiclePlate(this string plate)
        {
            if (string.IsNullOrWhiteSpace(plate))
                return null;

            Regex rgx = new Regex("[^a-zA-Z0-9]");
            return rgx.Replace(plate, "");
        }

        public static bool IsValidVehiclePlate(this string plate)
        {
            if (string.IsNullOrWhiteSpace(plate))
                return false;

            var clearedPlate = ClearVehiclePlate(plate);

            var regex = new Regex(@"^[A-Z]{3}[0-9]{1}[A-Z]{1}[0-9]{2}|[A-Z]{3}[0-9]{4}$");

            if (regex.IsMatch(clearedPlate.ToUpper()))
                return true;

            return false;
        }

        public static string ToPhoneFormat(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var ddd = value.Substring(0, 2);
            var number = value.Substring(2);

            return $"({ddd}) {number:# ####-####}";
        }

        public static string GetPhoneWithDDD(this string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone)) return null;

            var ddd = telefone.ClearPhone();
            return ddd.Substring(0, 2);
        }

        public static string GetPhoneWithoutDDD(this string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone)) return null;

            var telefoneSemDDD = telefone.ClearPhone();
            return telefoneSemDDD.Substring(2, telefoneSemDDD.Length - 2);
        }

        public static string ClearPhone(this string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return string.Empty;

            return telefone.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static string ClearIfNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s) ? string.Empty : s;
        }

        public static string NullIfEmpty(this string s)
        {
            return s == string.Empty ? null : s;
        }

        public static string EmptyIfNull(this string s)
        {
            return s ?? string.Empty;
        }

        public static string Truncate(this string value, int maxLenght, string legend = "...")
        {
            if (maxLenght == 0)
                throw new ArgumentException();

            var fim = 0;

            legend = string.IsNullOrWhiteSpace(legend) ? "" : legend;

            if (maxLenght > legend.Length)
                fim = legend.Length;

            if (!string.IsNullOrWhiteSpace(value) && value.Length > maxLenght)
                return value.Substring(0, maxLenght - fim) + (fim > 0 ? legend : "");

            return value;
        }

        public static string ToTitleCase(this string value, CultureInfo culture = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            return culture.TextInfo.ToTitleCase(value.ToLower());
        }

        public static string Cut(this string s, string initialToken, string finalToken)
        {
            int finishIndex;
            return s.Cut(initialToken, finalToken, 0, out finishIndex);
        }

        public static string Cut(this string s, string initialToken, string finalToken, out int finishIndex)
        {
            return s.Cut(initialToken, finalToken, 0, out finishIndex);
        }

        public static string Cut(this string s, string initialToken, string finalToken, int startIndex)
        {
            int finishIndex;
            return s.Cut(initialToken, finalToken, startIndex, out finishIndex);
        }

        public static string Cut(this string s, string initialToken, string finalToken, int startIndex, out int finishIndex)
        {
            if (string.IsNullOrWhiteSpace(initialToken))
                throw new ArgumentNullException("initialToken");

            if (string.IsNullOrWhiteSpace(finalToken))
                throw new ArgumentNullException("finalToken");

            if (string.IsNullOrWhiteSpace(s))
            {
                finishIndex = -1;
                return null;
            }

            var initialIndex = s.IndexOf(initialToken, startIndex, StringComparison.InvariantCultureIgnoreCase) + initialToken.Length;
            var finalIndex = s.IndexOf(finalToken, initialIndex, StringComparison.InvariantCultureIgnoreCase);
            finishIndex = finalIndex + finalToken.Length;

            return s.Substring(initialIndex, finalIndex - initialIndex);
        }

        public static string ToCamelCaseWord(this string value)
        {
            if (value.ToUpper() == value)
                return value.ToLower();

            return string.Format("{0}{1}", value.Substring(0, 1).ToLower(), value.Substring(1));
        }

        public static string ToPascalCaseWord(this string value)
        {
            return string.Format("{0}{1}", value.Substring(0, 1).ToUpper(), value.Substring(1));
        }

        public static string ToFirstLetterUperCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            return $"{value.First().ToString().ToUpper()}{value.Substring(1).ToLower()}";
        }

        public static string ToSlugstring(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            const int maxLen = 80;

            var valueLength = value.Length;
            var prevDash = false;
            var sb = new StringBuilder(valueLength);

            for (var i = 0; i < valueLength; i++)
            {
                var c = value[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevDash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    sb.Append((char)(c | 32)); // ToLower
                    prevDash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' || c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevDash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevDash = true;
                    }
                }
                else if (c >= 128)
                {
                    var prevlen = sb.Length;
                    sb.Append(c.ToAscii());
                    if (prevlen != sb.Length) prevDash = false;
                }
                if (i == maxLen) break;
            }

            if (prevDash)
                return sb.ToString().Substring(0, sb.Length - 1);

            return sb.ToString();
        }

        public static string ToAscii(this char c)
        {
            var s = c.ToString().ToLowerInvariant();

            if ("àåáâäãåą".Contains(s))
                return "a";

            if ("èéêëę".Contains(s))
                return "e";

            if ("ìíîïı".Contains(s))
                return "i";

            if ("òóôõöøőð".Contains(s))
                return "o";

            if ("ùúûüŭů".Contains(s))
                return "u";

            if ("çćčĉ".Contains(s))
                return "c";

            if ("żźž".Contains(s))
                return "z";

            if ("śşšŝ".Contains(s))
                return "s";

            if ("ñń".Contains(s))
                return "n";

            if ("ýÿ".Contains(s))
                return "y";

            if ("ğĝ".Contains(s))
                return "g";

            if (c == 'ř')
                return "r";

            if (c == 'ł')
                return "l";

            if (c == 'đ')
                return "d";

            if (c == 'ß')
                return "ss";

            if (c == 'Þ')
                return "th";

            if (c == 'ĥ')
                return "h";

            if (c == 'ĵ')
                return "j";

            return "";
        }

        public static bool GreaterThan(this string value, int length)
        {
            return value != null && value.Length > length;
        }

        public static bool IsValidEmailAddress(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return new Regex("\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*").Match(value).Success;
        }

        public static bool IsValidUrlHttp(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return value.ToLower().Contains("http://");
        }

        public static IEnumerable<string> EnumerateByLength(this string text, int length)
        {
            var index = 0;
            while (index < text.Length)
            {
                var charCount = Math.Min(length, text.Length - index);
                yield return text.Substring(index, charCount);
                index += length;
            }
        }

        public static bool IsHTMLContent(this string value)
        {
            var regex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");

            return regex.IsMatch(value);
        }

        public static string RemoveDiacritics(this string text)
        {
            return string.Concat(
                text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                              UnicodeCategory.NonSpacingMark)
              ).Normalize(NormalizationForm.FormC);
        }

        public static string VerifyInformedData(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "Não informado";
            }

            return value;
        }

        public static string ClearNumberMask(this string value)
        {
            try
            {
                Regex regexObj = new Regex(@"[^\d]");
                return regexObj.Replace(value, "");
            }
            catch
            {
                return value;
            }
        }

        public static string GetFirstName(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BusinessException("Não é possíivel extrair o primeiro nome de uma string nula ou vazia");

            var listValues = value.Split(" ");

            if (listValues.Count() < 2)
                throw new BusinessException($"O valor {value} não representa um nome completo");

            return listValues[0];
        }

        public static string GetLastName(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BusinessException("Não é possíivel extrair o sobrenome de uma string nula ou vazia");

            var listValues = value.Split(" ");

            if (listValues.Count() < 2)
                throw new BusinessException($"O valor {value} não representa um nome completo");

            return value.Substring(value.IndexOf(" ") + 1);
        }

        public static string GetBrazilianCurrency(this decimal amount)
        {
            return amount.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
        }
    }
}
