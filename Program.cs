using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace readSTL;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "STL_examples\\HK_v2.STL"; // Provide the path to your STL file

        if (File.Exists(filePath))
        {
            try
            {
                List<Vector3> vertices = new List<Vector3>();
                List<Triangle> triangles = new List<Triangle>();

                using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    // Read 80-byte header
                    reader.ReadBytes(80);

                    // Read the number of triangles (faces)
                    uint numTriangles = reader.ReadUInt32();

                    for (int i = 0; i < numTriangles; i++)
                    {
                        // Read the normal vector of the triangle (unused)
                        reader.ReadBytes(12);

                        // Read vertices of the triangle
                        Vector3[] triangleVertices = new Vector3[3];
                        for (int j = 0; j < 3; j++)
                        {
                            float x = reader.ReadSingle();
                            float y = reader.ReadSingle();
                            float z = reader.ReadSingle();

                            Vector3 vertex = new Vector3(x, y, z);
                            vertices.Add(vertex);
                            triangleVertices[j] = vertex;
                        }

                        triangles.Add(new Triangle(triangleVertices));

                        // Read attribute byte count (unused)
                        reader.ReadUInt16();
                    }
                }

                // Display vertices
                Console.WriteLine("Vertices:");
                foreach (var vertex in vertices)
                {
                    Console.WriteLine($"X: {vertex.X}, Y: {vertex.Y}, Z: {vertex.Z}");
                }

                // Display triangles
                Console.WriteLine("Triangles:");
                foreach (var triangle in triangles)
                {
                    Console.WriteLine($"Vertex 1: {triangle.Vertices[0]}, Vertex 2: {triangle.Vertices[1]}, Vertex 3: {triangle.Vertices[2]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading STL file: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("File not found.");
        }
    }
}

public class Triangle
{
    public Vector3[] Vertices { get; }

    public Triangle(Vector3[] vertices)
    {
        if (vertices.Length != 3)
        {
            throw new ArgumentException("A triangle must have exactly 3 vertices.");
        }

        Vertices = vertices;
    }
}

