using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System;

public class BTree
{
    private BTNode m_root;
    private BTContext m_context;
    private XDocument XMLTree;

    public BTree(BTNode root, Piece piece = null)
    {
        m_root = root;
        m_context = new BTContext();

        if (piece != null)
        {
            m_context.SetVariable("piece", piece);
        }

        m_root.Init(m_context);
    }

    public BTree(string XMLTreePath, Piece piece)
    {
        XMLTree = XDocument.Load(XMLTreePath);
        m_context = new BTContext();

        m_context.SetVariable("piece", piece);

        m_root = Create(XMLTree.Root);
        m_root.Init(m_context);
    }

    public BTNode Create(XElement current)
    {
        Piece piece = (Piece)m_context.GetVariable("piece");
        Type pieceType = piece.GetType();

        Type type = Type.GetType(current.Name.LocalName);

        if (type != null)
        {
            BTNode Node = (BTNode)Activator.CreateInstance(type);

            if (Node is BTContextSetter setter)
            {
                if (current.Attribute("context") != null)
                {
                    MethodInfo contextMethod = pieceType.GetMethod(current.Attribute("context").Value);
                    BTContextSetter.NewContext newContext = (BTContextSetter.NewContext)Delegate.CreateDelegate(typeof(BTContextSetter.NewContext), piece, contextMethod);
                    setter.SetContext(newContext);
                }
            }

            if (current.IsEmpty)    // is leaf node
            {
                return Node;
            }
            else        // is composite or decorator
            {
                foreach (XElement element in current.Elements())        // in case of decorator, this will only run once
                {
                    Node.AddChild(Create(element));
                }
                return Node;
            }
        }
        else { throw new Exception("Trying to create a BTNode of an unavailable type??"); }
    }

    public void SetPiece(Piece piece)
    {
        m_context.SetVariable("piece", piece);
    }

    public void Tick()
    {
        m_root.Tick(m_context);
    }
}

public class DebugXDocument
{
    XDocument test;

    public DebugXDocument(string XMLfile)
    {
        test = XDocument.Load(XMLfile);

        Debug.Log(test.Root.Name.LocalName);
    }
}
