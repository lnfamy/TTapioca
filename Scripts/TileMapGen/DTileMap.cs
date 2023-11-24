using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class DTileMap
{

	int size_x;
	int size_y;
	private Grid gr;
	private TileMap tileMap;
	Vector3[] vertices;
	int meshW, meshH, level;

	public GameObject aStar, floorMap;

	public int[,] map_data;
	private bool change = false;


	enum FLOORTYPES
	{
		grass	= 0,
		dirt = 1,
		gravelstone = 2,
		ice = 3,
		wood = 4,
		redwood = 5,
		cottontan = 6,

	
	}


	public DTileMap (int size_y, int size_x, int level)
	{
		this.size_x = size_x;
		this.size_y = size_y;
		int cells = size_x * size_y;
		this.level = level;
		aStar = GameObject.FindGameObjectWithTag ("aStar");
		gr = aStar.GetComponent<Grid> ().GetInstance ();
		floorMap = GameObject.FindGameObjectWithTag ("TileMap");
		tileMap = floorMap.GetComponent<TileMap> ().GetInstance ();
		UnityEngine.Debug.Log (cells);

		map_data = new int[size_x, size_y];
		if (level == 1) {
			MakeFirstLevel ();	
		} else if (level == 2) {
			MakeSecondLevel ();
		}
	}


	public int GetTileAt (int x, int y)
	{
		return map_data [x, y];
	}

	public void TextureRoom (int left, int top, int width, int height, bool notpink, int tex, bool wall)
	{
		if (notpink) {
			for (int j = 0; j < width; j++) {
				for (int k = 0; k < height; k++) {
					if (tex != (int)FLOORTYPES.gravelstone && tex != (int)FLOORTYPES.grass) {
						gr.ChangeWalkable ( left+ j,top+k, true);
					}
					map_data [top + k, left + j] = tex;
				}
			}
		} else if (!wall) {
			for (int i = 0; i < width; i++) {
				for (int n = 0; n < height; n++) {
					if (change) {
						map_data [top + n, left + i] = (int)FLOORTYPES.wood;
						change = false;
					} else {
						map_data [top + n, left + i] = (int)FLOORTYPES.redwood;
						change = true;
					}
					gr.ChangeWalkable (left+i,top+n,true);
				}
				if (change)
					change = false;
				else
					change = true;


			}

		} else if (wall) {
			for (int g = 0; g < width; g++) {
				for (int q = 0; q < height; q++) {
					if (g == 0 || g == width - 1 || q == 0 || q == height - 1) {
						map_data [top + q, left + g] = (int)FLOORTYPES.redwood;
					} else {
						map_data [top + q, left + g] = (int)FLOORTYPES.cottontan;
					}
					gr.ChangeWalkable (left+g,top+q,true);

				}
			}
		}
	}


	//this isn't necessary since i could technically make all corridors with MakeRoom but it's for my own peace of mind.
	public void TextureCorridor (int left, int top, int width, int height, int texture)
	{
		if (width == 1 && height > 1) {
			for (int i = 0; i < height; i++) {
				map_data [top + i, left] = texture;
				if (texture != (int)FLOORTYPES.gravelstone && texture != (int)FLOORTYPES.grass) {
					gr.ChangeWalkable (left,top+i, true);
				}
			}
		} else if (height == 1 && width > 1) {
			for (int n = 0; n < width; n++) {
				map_data [top, left + n] = texture;
				if (texture != (int)FLOORTYPES.gravelstone && texture != (int)FLOORTYPES.grass) {
					gr.ChangeWalkable ( left + n,top, true);
				} else {
					
				}
			}
		} else {
			map_data [top, left] = texture;
			if (texture != (int)FLOORTYPES.gravelstone && texture != (int)FLOORTYPES.grass) {
				gr.ChangeWalkable (left,top, true);
			}
		}
	}

	public Vector3[,] GetPositions ()
	{
		Vector3[,] points = new Vector3[18, 24];
		meshW = 100 * 24;
		meshH = 100 * 18;
		for (int x = 0; x < meshW; x += 100) {
			for (int y = 0; y < meshH; y += 100) {
				points [x, y] = new Vector3 (x, 0.0f, y);
			}
		}
		return points;
	}

	public void MakeFirstLevel (){
		TextureRoom (4,6,11,7,true,(int)FLOORTYPES.dirt,false);//middle room
		TextureRoom (7,1,5,3,true,(int)FLOORTYPES.cottontan,false); // spawn room
		TextureRoom(2,17,5,4,true,(int)FLOORTYPES.cottontan,false); // finish room
		TextureRoom(7,8,5,3,true,(int)FLOORTYPES.cottontan,false); // middle room surrounding button
		TextureCorridor(8,2,3,1,(int)FLOORTYPES.ice); // spawn room ice
		TextureCorridor(3,18,3,1,(int)FLOORTYPES.ice); //finish room ice
		TextureCorridor(8,9,3,1,(int)FLOORTYPES.ice); // middle room ice next to button
		TextureCorridor(9,3,1,4,(int)FLOORTYPES.wood); //first corridor next to spawn room
		TextureCorridor(6,12,1,2,(int)FLOORTYPES.wood); // top part of sec corridor from middle room
		TextureCorridor(5,14,7,1,(int)FLOORTYPES.wood); // mid part of hallway without edges
		TextureCorridor(12,14,1,6,(int)FLOORTYPES.wood); // right corridor without redwood part
		TextureCorridor(13,15,3,1,(int)FLOORTYPES.redwood); // redwood part leading to button
		TextureCorridor(4,14,1,4,(int)FLOORTYPES.wood); // left corridor
		TextureCorridor(9,2,1,1,(int)FLOORTYPES.redwood); // spawn point
		TextureCorridor(4,18,1,1,(int)FLOORTYPES.redwood); // finish point
	}




	public void MakeSecondLevel ()
	{
		TextureCorridor (9, 1, 1, 3, (int)FLOORTYPES.wood);
		TextureCorridor (6, 4, 8, 1, (int)FLOORTYPES.wood);
		TextureCorridor (9, 5, 1, 1, (int)FLOORTYPES.wood);
		TextureCorridor (6, 6, 7, 1, (int)FLOORTYPES.cottontan);
		TextureRoom (5, 7, 9, 3, true, (int)FLOORTYPES.cottontan, false);
		TextureCorridor (6, 10, 8, 1, (int)FLOORTYPES.cottontan);
		TextureCorridor (6, 11, 1, 1, (int)FLOORTYPES.gravelstone); //unwalkable
		TextureRoom (7, 7, 5, 3, false, 0, true);
		TextureCorridor (2, 8, 4, 1, (int)FLOORTYPES.wood);
		TextureCorridor (2, 7, 1, 1, (int)FLOORTYPES.wood);
		TextureRoom (1, 5, 3, 2, false, 0, false);
		TextureCorridor (14, 8, 1, 1, (int)FLOORTYPES.gravelstone); // unwalkable
		TextureRoom (10, 13, 3, 3, true, (int)FLOORTYPES.wood, false);
		TextureRoom (6, 13, 3, 3, true, (int)FLOORTYPES.redwood, false);
		TextureCorridor (8, 13, 3, 1, (int)FLOORTYPES.ice);
		TextureCorridor (9, 11, 1, 3, (int)FLOORTYPES.cottontan);
		TextureCorridor (5, 15, 1, 1, (int)FLOORTYPES.gravelstone); // unwalkable
		TextureCorridor (10, 16, 1, 1, (int)FLOORTYPES.gravelstone); // unwalkable
		TextureRoom (2, 15, 3, 2, false, 0, false);
		TextureRoom (10, 17, 3, 2, false, 0, false);
		TextureCorridor (13, 18, 2, 1, (int)FLOORTYPES.ice);
		TextureCorridor (15, 18, 1, 4, (int)FLOORTYPES.cottontan);
		TextureCorridor (13, 18, 2, 1, (int)FLOORTYPES.ice);
		TextureCorridor (15, 19, 1, 2, (int)FLOORTYPES.ice);
		TextureCorridor (12, 19, 1, 3, (int)FLOORTYPES.cottontan);
		TextureCorridor (12, 20, 1, 1, (int)FLOORTYPES.gravelstone); // unwalkable
		TextureCorridor (13, 21, 2, 1, (int)FLOORTYPES.ice);
		TextureCorridor (4, 17, 1, 1, (int)FLOORTYPES.gravelstone); // unwalkable
		TextureRoom (2, 18, 5, 3, true, (int)FLOORTYPES.cottontan, false);
		TextureCorridor (3, 19, 3, 1, (int)FLOORTYPES.wood);
		TextureCorridor (4, 19, 1, 1, (int)FLOORTYPES.ice);


	}
}
