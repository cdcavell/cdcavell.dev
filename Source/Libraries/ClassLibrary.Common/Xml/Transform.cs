using System.Xml.Xsl;

namespace ClassLibrary.Common.Xml
{ 
    /// <summary>
    /// Class to perform XSLT Transformation
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.0.0 | 08/20/2022 | Initial Development |~ 
    /// </revision>
    public class Transform
    {
        /// <summary>
        /// Method to perform XSLT Transformation and write out file
        /// </summary>
        /// <param name="inputXslt">string</param>
        /// <param name="inputXml">string</param>
        /// <param name="outputFile">string</param>
        /// <method>Write(string inputXslt, string inputXml, string outputFile)</method>
        public static void Write(string inputXslt, string inputXml, string outputFile)
        {
            XslCompiledTransform transform = new();
            transform.Load(inputXslt);
            transform.Transform(inputXml, outputFile);
        }
    }
}
