/******************************************************************************************************************/
/******* ¿Qué pasa si debemos soportar un nuevo idioma para los reportes, o agregar más formas geométricas? *******/
/******************************************************************************************************************/

/*
 * TODO: 
 * Refactorizar la clase para respetar principios de la programación orientada a objetos.
 * Implementar la forma TrapecioIsosceles/Rectangulo. 
 * Agregar el idioma Italiano (o el deseado) al reporte.
 * Se agradece la inclusión de nuevos tests unitarios para validar el comportamiento de la nueva funcionalidad agregada (los tests deben pasar correctamente al entregar la solución, incluso los actuales.)
 * Una vez finalizado, hay que subir el código a un repo GIT y ofrecernos la URL para que podamos utilizar la nueva versión :).
 */

using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WildarChallenge.Data.Resources;
using static Microsoft.Extensions.Localization.IStringLocalizer;
namespace WildarChallenge.Data.Classes
{
    public class FormaGeometrica
    {
        #region Formas
        // PODRIA HACERSE POLIMORFISMO ENTRE FIGURAS
        public const int Cuadrado = 1;
        public const int TrianguloEquilatero = 2;
        public const int Circulo = 3;
        public const int TrapecioIsosceles = 4;
        public const int Rectangulo = 5;
        #endregion

        #region Idiomas
        public const int Castellano = 1;
        public const int Ingles = 2;
        public const int Italiano = 3;
        #endregion

        private readonly decimal _lado;
        private readonly decimal? _anchoSup;
        private readonly decimal? _anchoInf;
        public int Tipo { get; set; }
        private static StringLocalizer<Resource> localizer ;
        public FormaGeometrica(int tipo, decimal lado, decimal? anchoSup, decimal? anchoInf)
        {
            Tipo = tipo;
            _lado = lado;
           _anchoSup = anchoSup;
           _anchoInf = anchoInf;
        }

        public static string Imprimir(List<FormaGeometrica> formas, int idioma)
        {
            var sb = new StringBuilder();
            string culture = "";
            switch (idioma)
            {
                case Ingles:
                    culture = "en-US";
                    break;
                case Castellano:
                    culture = "es-ES";
                    break;
                case Italiano:
                    culture = "it-IT";
                    break;
                default:
                    culture = "en-US";
                   break;
            }
            var cultureDefault = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = cultureDefault;
            CultureInfo.DefaultThreadCurrentUICulture = cultureDefault;



            if (!formas.Any())
            {
                sb.Append("<h1>" + localizer["EmptyList"] + "</h1>"); // HACER LO MISMO CON TODOS LOS LABELS
            }
            else
            {
              
                // Hay por lo menos una forma
                // HEADER
              
                    sb.Append("<h1>"+ localizer["Header"]  + "</h1>");

                var numeroCuadrados = 0;
                var numeroCirculos = 0;
                var numeroTriangulos = 0;

                var areaCuadrados = 0m;
                var areaCirculos = 0m;
                var areaTriangulos = 0m;

                var perimetroCuadrados = 0m;
                var perimetroCirculos = 0m;
                var perimetroTriangulos = 0m;

                for (var i = 0; i < formas.Count; i++)
                {
                    if (formas[i].Tipo == Cuadrado)
                    {
                        numeroCuadrados++;
                        areaCuadrados += formas[i].CalcularArea();
                        perimetroCuadrados += formas[i].CalcularPerimetro();
                    }
                    if (formas[i].Tipo == Circulo)
                    {
                        numeroCirculos++;
                        areaCirculos += formas[i].CalcularArea();
                        perimetroCirculos += formas[i].CalcularPerimetro();
                    }
                    if (formas[i].Tipo == TrianguloEquilatero)
                    {
                        numeroTriangulos++;
                        areaTriangulos += formas[i].CalcularArea();
                        perimetroTriangulos += formas[i].CalcularPerimetro();
                    }
                }
                
                sb.Append(ObtenerLinea(numeroCuadrados, areaCuadrados, perimetroCuadrados, Cuadrado, idioma));
                sb.Append(ObtenerLinea(numeroCirculos, areaCirculos, perimetroCirculos, Circulo, idioma));
                sb.Append(ObtenerLinea(numeroTriangulos, areaTriangulos, perimetroTriangulos, TrianguloEquilatero, idioma));

                // FOOTER
                sb.Append("TOTAL:<br/>");
                sb.Append(numeroCuadrados + numeroCirculos + numeroTriangulos + " " + (idioma == Castellano ? "formas" : "shapes") + " ");
                sb.Append((idioma == Castellano ? "Perimetro " : "Perimeter ") + (perimetroCuadrados + perimetroTriangulos + perimetroCirculos).ToString("#.##") + " ");
                sb.Append("Area " + (areaCuadrados + areaCirculos + areaTriangulos).ToString("#.##"));
            }

            return sb.ToString();
        }

        private static string ObtenerLinea(int cantidad, decimal area, decimal perimetro, int tipo, int idioma)
        {
            if (cantidad > 0)
            {
                if (idioma == Castellano)
                    return $"{cantidad} {TraducirForma(tipo, cantidad, idioma)} | Area {area:#.##} | Perimetro {perimetro:#.##} <br/>";

                return $"{cantidad} {TraducirForma(tipo, cantidad, idioma)} | Area {area:#.##} | Perimeter {perimetro:#.##} <br/>";
            }

            return string.Empty;
        }

        private static string TraducirForma(int tipo, int cantidad, int idioma)
        {
            switch (tipo)
            {
                case Cuadrado:
                    if (idioma == Castellano) return cantidad == 1 ? "Cuadrado" : "Cuadrados";
                    else return cantidad == 1 ? "Square" : "Squares";
                case Circulo:
                    if (idioma == Castellano) return cantidad == 1 ? "Círculo" : "Círculos";
                    else return cantidad == 1 ? "Circle" : "Circles";
                case TrianguloEquilatero:
                    if (idioma == Castellano) return cantidad == 1 ? "Triángulo" : "Triángulos";
                    else return cantidad == 1 ? "Triangle" : "Triangles";
            }

            return string.Empty;
        }

        public decimal CalcularArea()
        {
            switch (Tipo)
            {
                case Cuadrado: return _lado * _lado;
                case Circulo: return (decimal)Math.PI * (_lado / 2) * (_lado / 2);
                case TrianguloEquilatero: return ((decimal)Math.Sqrt(3) / 4) * _lado * _lado;
                case Rectangulo: return _lado * (decimal)_anchoInf;
                case TrapecioIsosceles: return (decimal)(_anchoInf + _anchoSup)*_lado/2 ;
                default:
                    throw new ArgumentOutOfRangeException(@"Forma desconocida");
            }
        }
        public decimal CalcularPerimetro()
        {
            switch (Tipo)
            {
                case Cuadrado: return _lado * 4;
                case Circulo: return (decimal)Math.PI * _lado;
                case TrianguloEquilatero: return _lado * 3;
                case Rectangulo: return (_lado * 2) + ((decimal)_anchoInf *2);
                case TrapecioIsosceles: return (_lado * 2) + (decimal)_anchoInf  + (decimal)_anchoSup ;
                default:
                    throw new ArgumentOutOfRangeException(@"Forma desconocida");
            }
        }
    }
}
