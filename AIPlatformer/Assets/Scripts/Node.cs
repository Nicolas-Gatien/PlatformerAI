public class Node
{
    public Node(double value, double weight, double bias)
    {
        this.value = value;
        this.weight = weight;
        this.bias = bias;
    }
    public Node()
    {

    }
    public double value;

    public double _weight;
    public double _bias;

    public double weight
    {
        set
        {
            if (value > 2)
            {
                _weight = 2;
            }else if (value < -2)
            {
                _weight = -2;
            }else
            {
                _weight = value;
            }
            
        }

        get
        {
            return _weight;
        }
    }
    public double bias
    {
        set
        {
            if (value > 5)
            {
                _bias = 5;
            }
            else if (value < -5)
            {
                _bias = -5;
            }
            else
            {
                _bias = value;
            }

        }

        get
        {
            return _bias;
        }
    }
}