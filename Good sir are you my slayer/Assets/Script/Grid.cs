using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public Transform Player;
	public LayerMask UnwalkableMask;
	public Vector2 GridWorldSize;
	public float NodeRadius;
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Start()
	{
		nodeDiameter = NodeRadius*2;
		gridSizeX = Mathf.RoundToInt(GridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(GridWorldSize.y/nodeDiameter);
		CreateGrid();
	}

	void CreateGrid()
	{
		grid = new Node[gridSizeX,gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x/2 - Vector3.up * GridWorldSize.y/2;
		
		for (int x = 0;x < gridSizeX;x++)
		{
			for (int y = 0;y < gridSizeY;y++)
			{
				Vector3 worldpoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + NodeRadius) + Vector3.up * (y * nodeDiameter + NodeRadius);
				bool walkable = !(Physics.CheckSphere(worldpoint,NodeRadius, UnwalkableMask));
				grid[x,y] = new Node(walkable,worldpoint);
			}
		}
	}

	public Node NodeFromWorlPoint(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x + GridWorldSize.x/2) / GridWorldSize.x;
		float percentY = (worldPosition.y + GridWorldSize.y/2) / GridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX -1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY -1) * percentY);
		return grid[x,y];
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x,GridWorldSize.y,1));

		if (grid != null)
		{
			Node playernode = NodeFromWorlPoint(Player.position);
			foreach (Node n in grid)
			{
				Gizmos.color = (n.Walkable)?Color.white:Color.red;
				if (playernode == n)
					Gizmos.color = Color.cyan;
				Gizmos.DrawCube(n.WorldPosition,Vector3.one * (nodeDiameter-0.1f));
			}
		}
	}
}
