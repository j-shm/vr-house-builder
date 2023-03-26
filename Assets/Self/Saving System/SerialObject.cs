
public class SerialObject
{
    public float[] position = new float[3]; 
    public float[] rotation = new float[3];
    public float[] scale = new float[3];
    public string name;

    public SerialObject(string name, float[] position,float[] rotation,float[] scale) {
        this.name = name;
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }


}
