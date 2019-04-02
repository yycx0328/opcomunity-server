// =========================================================
// Author	:   luyunhai
// Create Time  :   8/10/2015 10:56:23 PM
// =========================================================
// Copyright © USER-VFH583E7VU 2015 . All rights reserved.
// =========================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

/// <summary>
/// XmlHelper
/// </summary>
public static partial class XmlHelper
{
    /// <summary>
    /// XMLNS
    /// </summary>
    const string XMLNS = "xmlns";
    /// <summary>
    /// XSI
    /// </summary>
    const string XSI = "xsi";
    /// <summary>
    /// XSD
    /// </summary>
    const string XSD = "xsd";
    /// <summary>
    /// NAME_SPLIT
    /// </summary>
    const char NAME_SPLIT = ':';
    /// <summary>
    /// XMLNS_VALUE
    /// </summary>
    const string XMLNS_VALUE = "http://www.w3.org/2000/xmlns/";
    /// <summary>
    /// XMLNS_XSI_VALUE
    /// </summary>
    const string XMLNS_XSI_VALUE = "http://www.w3.org/2001/XMLSchema-instance";
    /// <summary>
    /// XMLNS_XSD_VALUE
    /// </summary>
    const string XMLNS_XSD_VALUE = "http://www.w3.org/2001/XMLSchema";

    /// <summary>
    /// XSL_URI
    /// </summary>
    public const string XSL_URI = "http://www.w3.org/1999/XSL/Transform";

    /* InvalidChar ::= [#x0-#x8] | #xB | #xC | [#xE-#x1F] | [#xD800-#xDFFF] | #xFFFE | #xFFFF */

    #region XmlAttribute
    /// <summary>
    /// GetAttribute
    /// </summary>
    /// <param name="source"></param>
    /// <param name="attributeName"></param>
    /// <param name="namespaceUri"></param>
    /// <param name="createIfMissing"></param>
    /// <returns></returns>
    public static XmlAttribute GetAttribute(XmlNode source, string attributeName, string namespaceUri, bool createIfMissing)
    {
        if (namespaceUri == null)
            namespaceUri = string.Empty;

        XmlAttribute attr = null;
        if (source != null && source.Attributes != null)
        {
            bool isLocalName = attributeName.IndexOf(':') < 0;
            if (isLocalName)
                attr = source.Attributes[attributeName, namespaceUri];
            else
                attr = source.Attributes[attributeName];

            if (createIfMissing && attr == null && source.OwnerDocument != null)
            {
                attr = source.OwnerDocument.CreateAttribute(attributeName, namespaceUri);
                source.Attributes.Append(attr);
            }
        }

        return attr;
    }

    /// <summary>
    /// GetAttribute
    /// </summary>
    /// <param name="source"></param>
    /// <param name="attributeName"></param>
    /// <returns></returns>
    public static XmlAttribute GetAttribute(XmlNode source, string attributeName)
    {
        return GetAttribute(source, attributeName, string.Empty, false);
    }

    /// <summary>
    /// GetAttributeValue
    /// </summary>
    /// <param name="source"></param>
    /// <param name="attributeName"></param>
    /// <param name="defaultValue"></param>
    /// <param name="namespaceUri"></param>
    /// <returns></returns>
    public static string GetAttributeValue(XmlNode source, string attributeName, string defaultValue, string namespaceUri)
    {
        XmlAttribute attr = GetAttribute(source, attributeName, namespaceUri, false);
        if (attr == null)
            return defaultValue;
        else
            return attr.Value;
    }

    /// <summary>
    /// GetAttributeValue
    /// </summary>
    /// <param name="source"></param>
    /// <param name="attributeName"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string GetAttributeValue(XmlNode source, string attributeName, string defaultValue)
    {
        return GetAttributeValue(source, attributeName, defaultValue, string.Empty);
    }

    /// <summary>
    /// GetAttributeValue
    /// </summary>
    /// <param name="source"></param>
    /// <param name="attributeName"></param>
    /// <returns></returns>
    public static string GetAttributeValue(XmlNode source, string attributeName)
    {
        return GetAttributeValue(source, attributeName, null, string.Empty);
    }

    /// <summary>
    /// RemoveAttribute
    /// </summary>
    /// <param name="source"></param>
    /// <param name="attributeName"></param>
    /// <param name="namespaceUri"></param>
    /// <returns></returns>
    public static XmlAttribute RemoveAttribute(XmlNode source, string attributeName, string namespaceUri)
    {
        XmlAttribute attr = GetAttribute(source, attributeName, namespaceUri, false);
        if (attr != null)
            return source.Attributes.Remove(attr);
        else
            return null;
    }

    /// <summary>
    /// RemoveAttribute
    /// </summary>
    /// <param name="source"></param>
    /// <param name="attributeName"></param>
    /// <returns></returns>
    public static XmlAttribute RemoveAttribute(XmlNode source, string attributeName)
    {
        return RemoveAttribute(source, attributeName, string.Empty);
    }

    /// <summary>
    /// RemoveAttribute
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public static XmlAttribute RemoveAttribute(XmlAttribute attribute)
    {
        if (attribute != null && attribute.OwnerElement != null)
            return attribute.OwnerElement.Attributes.Remove(attribute);
        else
            return null;
    }

    /// <summary>
    /// SetAttribute
    /// </summary>
    /// <param name="source"></param>
    /// <param name="attributeName"></param>
    /// <param name="attributeValue"></param>
    /// <param name="namespaceUri"></param>
    /// <returns></returns>
    public static XmlAttribute SetAttribute(XmlNode source, string attributeName, string attributeValue, string namespaceUri)
    {
        if (attributeValue == null)
        {
            RemoveAttribute(source, attributeName, namespaceUri);
            return null;
        }
        else
        {
            XmlAttribute attr = GetAttribute(source, attributeName, namespaceUri, true);
            if (attr != null)
                attr.Value = attributeValue;
            return attr;
        }
    }

    /// <summary>
    /// SetAttribute
    /// </summary>
    /// <param name="source"></param>
    /// <param name="attributeName"></param>
    /// <param name="attributeValue"></param>
    /// <returns></returns>
    public static XmlAttribute SetAttribute(XmlNode source, string attributeName, string attributeValue)
    {
        return SetAttribute(source, attributeName, attributeValue, string.Empty);
    }

    /// <summary>
    /// AddAttribute
    /// </summary>
    /// <param name="source"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static XmlAttribute AddAttribute(XmlNode source, XmlAttribute attr)
    {
        if (attr != null)
            return SetAttribute(source, attr.Name, attr.Value, attr.NamespaceURI);
        else
            return null;
    }
    /// <summary>
    /// AttributesToCollection
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static System.Collections.Specialized.NameValueCollection AttributesToCollection(XmlNode source)
    {
        System.Collections.Specialized.NameValueCollection nv = new System.Collections.Specialized.NameValueCollection();
        if (source != null)
        {
            foreach (XmlAttribute attr in source.Attributes)
            {
                nv.Add(attr.Name, attr.Value);
            }
        }
        return nv;
    }
    #endregion

    #region XmlNode
    /// <summary>
    /// GetNode
    /// </summary>
    /// <param name="source"></param>
    /// <param name="nodeName"></param>
    /// <param name="namespaceUri"></param>
    /// <param name="createIfMissing"></param>
    /// <returns></returns>
    public static XmlNode GetNode(XmlNode source, string nodeName, string namespaceUri, bool createIfMissing)
    {
        if (namespaceUri == null)
            namespaceUri = string.Empty;

        XmlNode node = null;
        if (source != null && source.ChildNodes != null)
        {
            bool isLocalName = nodeName.IndexOf(':') < 0;
            if (isLocalName)
                node = source[nodeName, namespaceUri];
            else
                node = source[nodeName];

            if (createIfMissing && node == null)
            {
                if (source.OwnerDocument == null)
                {
                    node = (source as XmlDocument).CreateElement(nodeName, namespaceUri);
                    source.AppendChild(node);
                }
                else
                {
                    node = source.OwnerDocument.CreateElement(nodeName, namespaceUri);
                    source.AppendChild(node);
                }
            }
        }

        return node;
    }

    /// <summary>
    /// GetNode
    /// </summary>
    /// <param name="source"></param>
    /// <param name="nodeName"></param>
    /// <returns></returns>
    public static XmlNode GetNode(XmlNode source, string nodeName)
    {
        return GetNode(source, nodeName, string.Empty, false);
    }

    /// <summary>
    /// GetInnerText
    /// </summary>
    /// <param name="source"></param>
    /// <param name="nodeName"></param>
    /// <param name="defaultValue"></param>
    /// <param name="namespaceUri"></param>
    /// <returns></returns>
    public static string GetInnerText(XmlNode source, string nodeName, string defaultValue, string namespaceUri)
    {
        XmlNode node = GetNode(source, nodeName, namespaceUri, false);
        if (node != null)
            return node.InnerText;
        else
            return defaultValue;
    }

    /// <summary>
    /// GetInnerText
    /// </summary>
    /// <param name="source"></param>
    /// <param name="nodeName"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string GetInnerText(XmlNode source, string nodeName, string defaultValue)
    {
        return GetInnerText(source, nodeName, defaultValue, string.Empty);
    }

    /// <summary>
    /// GetInnerText
    /// </summary>
    /// <param name="source"></param>
    /// <param name="nodeName"></param>
    /// <returns></returns>
    public static string GetInnerText(XmlNode source, string nodeName)
    {
        return GetInnerText(source, nodeName, null, string.Empty);
    }

    /// <summary>
    /// GetInnerXml
    /// </summary>
    /// <param name="source"></param>
    /// <param name="nodeName"></param>
    /// <param name="defaultValue"></param>
    /// <param name="namespaceUri"></param>
    /// <returns></returns>
    public static string GetInnerXml(XmlNode source, string nodeName, string defaultValue, string namespaceUri)
    {
        XmlNode node = GetNode(source, nodeName, namespaceUri, false);
        if (node != null)
            return node.InnerXml;
        else
            return defaultValue;
    }

    /// <summary>
    /// GetInnerXml
    /// </summary>
    /// <param name="source"></param>
    /// <param name="nodeName"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string GetInnerXml(XmlNode source, string nodeName, string defaultValue)
    {
        return GetInnerXml(source, nodeName, defaultValue, string.Empty);
    }

    /// <summary>
    /// GetInnerXml
    /// </summary>
    /// <param name="source"></param>
    /// <param name="nodeName"></param>        
    /// <returns></returns>
    public static string GetInnerXml(XmlNode source, string nodeName)
    {
        return GetInnerXml(source, nodeName, null, string.Empty);
    }

    /// <summary>
    /// DeleteNode
    /// </summary>
    /// <param name="source"></param>
    /// <param name="nodeName"></param>
    /// <param name="namespaceUri"></param>
    /// <returns></returns>
    public static XmlNode DeleteNode(XmlNode source, string nodeName, string namespaceUri)
    {
        XmlNode node = GetNode(source, nodeName, namespaceUri, false);
        if (node != null && node.ParentNode != null)
            return node.ParentNode.RemoveChild(node);
        else
            return null;
    }

    /// <summary>
    /// DeleteNode
    /// </summary>
    /// <param name="source"></param>
    /// <param name="nodeName"></param>
    /// <returns></returns>
    public static XmlNode DeleteNode(XmlNode source, string nodeName)
    {
        return DeleteNode(source, nodeName, string.Empty);
    }

    /// <summary>
    /// DeleteNode
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static XmlNode DeleteNode(XmlNode source)
    {
        if (source != null && source.ParentNode != null)
            return source.ParentNode.RemoveChild(source);
        else if (source is XmlAttribute)
            return RemoveAttribute(source as XmlAttribute);
        else
            return null;
    }

    /// <summary>
    /// SetInnerXml
    /// </summary>
    /// <param name="source"></param>
    /// <param name="nodeName"></param>
    /// <param name="innerXml"></param>
    /// <param name="namespaceUri"></param>
    /// <returns></returns>
    public static XmlNode SetInnerXml(XmlNode source, string nodeName, string innerXml, string namespaceUri)
    {
        if (innerXml == null)
        {
            DeleteNode(source, nodeName, namespaceUri);
            return null;
        }
        else
        {
            XmlNode node = GetNode(source, nodeName, namespaceUri, true);
            if (node != null)
                node.InnerXml = innerXml;
            return node;
        }
    }

    /// <summary>
    /// SetInnerXml
    /// </summary>
    /// <param name="source"></param>
    /// <param name="nodeName"></param>
    /// <param name="innerXml"></param>
    /// <returns></returns>
    public static XmlNode SetInnerXml(XmlNode source, string nodeName, string innerXml)
    {
        return SetInnerXml(source, nodeName, innerXml, string.Empty);
    }

    /// <summary>
    /// SetInnerText
    /// </summary>
    /// <param name="source"></param>
    /// <param name="nodeName"></param>
    /// <param name="innerText"></param>
    /// <param name="namespaceUri"></param>
    /// <returns></returns>
    public static XmlNode SetInnerText(XmlNode source, string nodeName, string innerText, string namespaceUri)
    {
        if (innerText == null)
        {
            DeleteNode(source, nodeName, namespaceUri);
            return null;
        }
        else
        {
            XmlNode node = GetNode(source, nodeName, namespaceUri, true);
            if (node != null)
                node.InnerText = innerText;
            return node;
        }
    }

    /// <summary>
    /// SetInnerText
    /// </summary>
    /// <param name="source"></param>
    /// <param name="nodeName"></param>
    /// <param name="innerText"></param>
    /// <returns></returns>
    public static XmlNode SetInnerText(XmlNode source, string nodeName, string innerText)
    {
        return SetInnerText(source, nodeName, innerText, string.Empty);
    }

    /// <summary>
    /// AddNode
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="child"></param>
    /// <returns></returns>
    public static XmlNode AddNode(XmlNode destination, XmlNode child)
    {
        if (destination == null || child == null)
            return null;

        bool isOwnerDocument = (destination is XmlDocument);
        XmlDocument destnationDocument = (isOwnerDocument ? (XmlDocument)destination : destination.OwnerDocument);
        XmlNode appendNode = CloneNode(destnationDocument, child, true);

        if (appendNode != null)
        {
            if (isOwnerDocument)
            {
                if (destnationDocument.DocumentElement == null)
                    destnationDocument.AppendChild(appendNode);
                else
                    destnationDocument.DocumentElement.AppendChild(appendNode);
            }
            else
                destination.AppendChild(appendNode);
        }

        return appendNode;
    }

    /// <summary>
    /// CloneNode
    /// </summary>
    /// <param name="refDocument"></param>
    /// <param name="node"></param>
    /// <param name="deep"></param>
    /// <returns></returns>
    public static XmlNode CloneNode(XmlDocument refDocument, XmlNode node, bool deep)
    {
        if (node == null)
            return null;

        bool isOwnerDocument = (node is XmlDocument);
        XmlDocument nodeDocument = (isOwnerDocument ? (XmlDocument)node : node.OwnerDocument);

        if (refDocument == null || refDocument == nodeDocument)
        {
            return node.CloneNode(deep);
        }
        else if (isOwnerDocument)
        {
            if (nodeDocument.DocumentElement == null)
                return null;
            else
                return refDocument.ImportNode(nodeDocument.DocumentElement, deep);
        }
        else
        {
            return refDocument.ImportNode(node, deep);
        }
    }

    /// <summary>
    /// Replace one XmlNode in a document with another.  If the replacement node is in a different document,
    ///     it is imported.  Otherwise, the replacement node is copied (deep).
    /// </summary>
    /// <param name="source">Node to replace</param>
    /// <param name="replacement">Replacement node</param>
    /// <returns>source node if it was removed from the tree, else null</returns>
    public static XmlNode ReplaceNode(XmlNode source, XmlNode replacement)
    {
        if (source == null)
            return null;

        if (replacement == null)
        {
            return DeleteNode(source);
        }
        else if (source.OwnerDocument != null)
        {
            XmlNode newNode = CloneNode(source.OwnerDocument, replacement, true);
            if (newNode == null)
                return DeleteNode(source);
            else if (source.ParentNode != null)
                return source.ParentNode.ReplaceChild(newNode, source);
        }

        return null;
    }

    /// <summary>
    /// Replaces the old tag in all the input nodes with the new tag
    /// </summary>
    /// <param name="node"></param>
    /// <param name="newTag"></param>
    /// <returns></returns>
    public static XmlNode ReplaceTag(XmlNode node, string newTag)
    {
        if (node == null || string.IsNullOrEmpty(newTag))
            return null;

        if (string.Equals(node.Name, newTag, StringComparison.InvariantCulture))
            return node;

        XmlDocument doc = node as XmlDocument;
        if (doc == null)
            doc = node.OwnerDocument;

        XmlNode newNode = doc.CreateElement(newTag, node.NamespaceURI);
        CopyNodeContents(newNode, node);
        if (node.ParentNode != null)
            node.ParentNode.ReplaceChild(newNode, node);

        return newNode;
    }

    /// <summary>
    /// this method copies the attributes and children of the source node to destination node
    /// </summary>
    /// <param name="destinationNode">destination node</param>
    /// <param name="sourceNode">source node</param>
    public static void CopyNodeContents(XmlNode destinationNode, XmlNode sourceNode)
    {
        if (destinationNode == null || sourceNode == null)
            return;

        if (sourceNode.ChildNodes != null)
        {
            foreach (XmlNode node in sourceNode.ChildNodes)
            {
                AddNode(destinationNode, node);
            }
        }

        if (sourceNode.Attributes != null)
        {
            foreach (XmlAttribute attr in sourceNode.Attributes)
            {
                AddAttribute(destinationNode, attr);
            }
        }
    }

    /// <summary>
    /// CreateXmlDocument
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static XmlDocument CreateXmlDocument(XmlNode node)
    {
        XmlDocument resultDoc = new XmlDocument();

        XmlNode childNode = null;
        if (node is XmlDocument)
            childNode = CloneNode(resultDoc, (node as XmlDocument).DocumentElement, true);
        else if (node is XmlElement)
            childNode = CloneNode(resultDoc, node, true);

        if (childNode != null)
            resultDoc.AppendChild(childNode);

        return resultDoc;
    }

    /// <summary>
    /// CreateXmlDocument
    /// </summary>
    /// <param name="rootName"></param>
    /// <param name="rootNameSpace"></param>
    /// <returns></returns>
    public static XmlDocument CreateXmlDocument(string rootName, string rootNameSpace)
    {
        if (string.IsNullOrEmpty(rootName))
            return null;

        XmlDocument resultDoc = new XmlDocument();

        XmlNode root = null;
        if (string.IsNullOrEmpty(rootNameSpace))
            root = resultDoc.CreateElement(rootName);
        else
            root = resultDoc.CreateElement(rootName, rootNameSpace);
        resultDoc.AppendChild(root);

        return resultDoc;
    }

    /// <summary>
    /// DataTableToXml
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    public static XmlDocument DataTableToXml(System.Data.DataTable table)
    {
        if (table == null)
            return null;

        if (string.IsNullOrEmpty(table.TableName))
            table.TableName = "Table1";

        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
        {
            table.WriteXml(ms);

            ms.Seek(0, System.IO.SeekOrigin.Begin);
            XmlDocument doc = new XmlDocument();
            doc.Load(ms);
            return doc;
        }
    }

    /// <summary>
    /// CreateXslCompiledTransform
    /// </summary>
    /// <param name="version"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public static System.Xml.Xsl.XslCompiledTransform CreateXslCompiledTransform(string version, XmlNode node)
    {
        if (node == null)
            return null;

        XmlDocument document = null;
        if (node is XmlDocument)
        {
            document = document.CloneNode(true) as XmlDocument;
        }
        else if (node is XmlElement && string.Equals(node.Name, "xsl:stylesheet", StringComparison.InvariantCulture))
        {
            document = CreateXmlDocument(node);
        }
        else
        {
            document = CreateXmlDocument("xsl:stylesheet", XSL_URI);
            SetAttribute(document.DocumentElement, "version", version);
        }

        System.Xml.Xsl.XslCompiledTransform transform = new System.Xml.Xsl.XslCompiledTransform();
        transform.Load(document.CreateNavigator(), null, new XmlUrlResolver());

        return transform;
    }

    /// <summary>
    /// Transforms an XmlNode to an XmlNode using the supplied transform.
    /// </summary>
    /// <param name="sourceNode">The XmlNode to transform.</param>
    /// <param name="transform">An XslCompiledTransform  object to transform with.</param>
    /// <param name="argumentList">An optional XsltArgumentList to use during the transform.  Null if none.</param>
    /// <param name="writerSettings"> specify the set of features you want to enable on the new XmlWriter object.</param>
    /// <returns>An XmlNode containing the transformed results or null</returns>        
    public static XmlDocument TransformToXml(XmlNode sourceNode, System.Xml.Xsl.XslCompiledTransform transform, System.Xml.Xsl.XsltArgumentList argumentList, XmlWriterSettings writerSettings)
    {
        if (sourceNode == null || transform == null)
            return null;

        XmlDocument resultsDom = new XmlDocument();
        if (sourceNode != null && transform != null)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                XmlTextWriter writer = (writerSettings == null ? XmlTextWriter.Create(ms) : XmlTextWriter.Create(ms, writerSettings)) as XmlTextWriter;
                transform.Transform(sourceNode.CreateNavigator(), argumentList, writer);

                if (ms != null)
                {
                    ms.Seek(0, System.IO.SeekOrigin.Begin);
                    resultsDom.Load(ms);
                }
            }
        }

        return resultsDom;
    }
    #endregion
}
