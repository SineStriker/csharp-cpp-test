namespace XSAppModel.XStudio;

public class LineParamNode
{
    public LineParamNode() : this(0, 0)
    {
    }

    public LineParamNode(int pos, int value)
    {
        Pos = pos;
        Value = value;
    }

    /* Properties */
    public int Pos;
    public int Value;
}

public class LineParam
{
    public LineParam()
    {
        nodeLinkedList = new LinkedList<LineParamNode>();
    }

    /* This class is derived from System.Runtime.Serialization.ISerializable
     * Using custom functions to pack or unpack data
     */

    public LinkedList<LineParamNode> nodeLinkedList;
}