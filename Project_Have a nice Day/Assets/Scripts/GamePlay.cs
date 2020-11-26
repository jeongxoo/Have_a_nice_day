using UnityEngine;
using System.Collections;

public class GamePlay : MonoBehaviour
{

	public GameObject TilePrefab = null; // 만들 블록의 프리팹.
	public TileControl[,] tiles; // 그리드.

	private GameObject main_camera = null; // 메인 카메라.
	private TileControl grabbed_tile = null; // 잡은 블록.


	void Start()
	{
		this.main_camera = GameObject.FindGameObjectWithTag("MainCamera");
	}


	void Update()
	{
		Vector3 mouse_position; 
		this.unprojectMousePosition(out mouse_position, Input.mousePosition);
		
		Vector2 mouse_position_xy =	new Vector2(mouse_position.x, mouse_position.y);
		if (this.grabbed_tile == null)
		{ 
		  
			if (Input.GetMouseButtonDown(0))
			{ 
				foreach (TileControl tile in this.tiles)
				{
					if (!tile.isGrabbable())
					{ 
						continue;
					}
					
					if (!tile.isContainedPosition(mouse_position_xy))
					{
						continue;
					}
					
					this.grabbed_tile = tile;
					this.grabbed_tile.beginGrab();
					break;
				}
			}
		}
		else
		{ 
			do
			{
				TileControl swap_target = this.getNextTile(grabbed_tile, grabbed_tile.slide_dir);
				if (swap_target == null)
				{
					break; 
				}
				
				if (!swap_target.isGrabbable())
				{
					break;
				}
				float offset = this.grabbed_tile.calcDirOffset(mouse_position_xy, this.grabbed_tile.slide_dir);
				if (offset < Tile.COLLISION_SIZE / 2.0f)
				{
					break;
				}
				
				this.swapTile(grabbed_tile, grabbed_tile.slide_dir, swap_target);
				this.grabbed_tile = null; 
			} while (false);



			if (!Input.GetMouseButton(0))
			{
				this.grabbed_tile.endGrab(); 
				this.grabbed_tile = null;
			}
		}
	}







	// 블록을 만들어 내고 배치
	public void initialSetUp()
	{
		 
		this.tiles = new TileControl[Tile.TILE_NUM_X, Tile.TILE_NUM_Y]; //그리드의 크기
		
		int color_index = 4;
		int stg1_tile_red_x1 = 0; //Random.Range(0, 5); //1~5
		int stg1_tile_red_y1 = 0; //Random.Range(0, 5);
		int stg1_tile_red_x2 = Random.Range(0, 5); 
		int stg1_tile_red_y2 = Random.Range(0, 5);

		int stg1_tile_blue_x1 = Random.Range(0, 5); //1~5
		int stg1_tile_blue_y1 = Random.Range(0, 5);
		int stg1_tile_blue_x2 = Random.Range(0, 5);
		int stg1_tile_blue_y2 = Random.Range(0, 5);


		for (int y = 0; y < Tile.TILE_NUM_Y; y++)
		{ 
			for (int x = 0; x < Tile.TILE_NUM_X; x++)
			{
				GameObject game_object = Instantiate(this.TilePrefab) as GameObject;
				TileControl tile = game_object.GetComponent<TileControl>(); //
				this.tiles[x, y] = tile;
				tile.i_pos.x = x;
				tile.i_pos.y = y;
				tile.tile_root = this;
				Vector3 position = GamePlay.calcTilePosition(tile.i_pos);
				tile.transform.position = position;
				tile.setColor((Tile.COLOR)color_index);
				tile.name = "tile(" + tile.i_pos.x.ToString() +	"," + tile.i_pos.y.ToString() + ")";

				//색깔 올랜덤으로 다 넣기
				//color_index = Random.Range(0, (int)Tile.COLOR.NORMAL_COLOR_NUM);
				
				color_index = 4;//검정색
								//빨강0 파랑1 노랑2 연두3 검정4
				/*
				if(x == 0 && y == 0)
				{
					tile.setColor((Tile.COLOR)color_index);
					color_index = 4;
				}//임시로 버그 해결법
				*/
				
				// 중복문제 해결해야함.

				if ((x == stg1_tile_red_x1 && y == stg1_tile_red_y1)
					|| (x == stg1_tile_red_x2 && y == stg1_tile_red_y2))
				{
					tile.setColor((Tile.COLOR)color_index);
					color_index = 0; //빨강
				}
				if ((x == stg1_tile_blue_x1 && y == stg1_tile_blue_y1) 
					|| (x == stg1_tile_blue_x2 && y == stg1_tile_blue_y2))
				{
					tile.setColor((Tile.COLOR)color_index);
					color_index = 1; //파랑
				}
				



			}
		}
		
		
		//여기다가 지정색깔 넣기
		



		//탐색알고리즘을 통해서 tile의 배열값을 스캔한뒤에 그것이 맞으면 통과하는식으로 짜보자.
		//dfs로 ㄱ
	}


	// 그리드 좌표로 씬에서의 좌표를 구한다
	public static Vector3 calcTilePosition(Tile.iPosition i_pos) 
	{
		// 배치할 왼쪽 위 구석 위치를 초깃값으로 설정.
		Vector3 position = new Vector3(-(Tile.TILE_NUM_X / 2.0f - 0.5f),
									   -(Tile.TILE_NUM_Y / 2.0f - 0.5f), 0.0f);
		
		position.x += (float)i_pos.x * Tile.COLLISION_SIZE; // 초깃값 + 그리드 좌표× 블록 크기.
		position.y += (float)i_pos.y * Tile.COLLISION_SIZE;
		return (position); 
	}

	
	public bool unprojectMousePosition(out Vector3 world_position, Vector3 mouse_position)
	{
		bool ret;
		// 판을 생성. 이 판은 카메라에 대해서 뒤로 향해서(Vector3.back).
		// 블록의 절반 크기만큼 앞에 둔다.
		Plane plane = new Plane(Vector3.back, new Vector3(0.0f, 0.0f, -Tile.COLLISION_SIZE / 2.0f));
		// 카메라와 마우스를 통과하는 빛을 만든다.
		Ray ray = this.main_camera.GetComponent<Camera>().ScreenPointToRay(
			mouse_position);
		float depth;
		// 광선(ray)가 판(plane)에 닿았다면.
		if (plane.Raycast(ray, out depth))
		{
			// 인수 world_position을 마우스 위치로 덮어쓴다.
			world_position = ray.origin + ray.direction * depth;
			ret = true;
			// 닿지 않았다면.
		}
		else
		{
			// 인수 world_position를 0인 벡터로 덮어쓴다.
			world_position = Vector3.zero;
			ret = false;
		}
		return (ret);
	}




	public TileControl getNextTile(
		TileControl tile, Tile.DIR4 dir)
	{
		// 슬라이드할 곳의 타일을 여기에 저장.
		TileControl next_tile = null;
		switch (dir)
		{
			case Tile.DIR4.RIGHT:
				if (tile.i_pos.x < Tile.TILE_NUM_X - 1)
				{
					next_tile = this.tiles[tile.i_pos.x + 1, tile.i_pos.y];
				}
				break;

			case Tile.DIR4.LEFT:
				if (tile.i_pos.x > 0)
				{
					next_tile = this.tiles[tile.i_pos.x - 1, tile.i_pos.y];
				}
				break;
			case Tile.DIR4.UP:
				if (tile.i_pos.y < Tile.TILE_NUM_Y - 1)
				{ 
					next_tile = this.tiles[tile.i_pos.x, tile.i_pos.y + 1];
				}
				break;
			case Tile.DIR4.DOWN:
				if (tile.i_pos.y > 0)
				{
					next_tile = this.tiles[tile.i_pos.x, tile.i_pos.y - 1];
				}
				break;
		}
		return (next_tile);
	}

	public static Vector3 getDirVector(Tile.DIR4 dir)
	{
		Vector3 v = Vector3.zero;
		switch (dir)
		{
			case Tile.DIR4.RIGHT: v = Vector3.right; break; // 오른쪽으로 1단위 이동.
			case Tile.DIR4.LEFT: v = Vector3.left; break; // 왼쪽으로 1단위 이동.
			case Tile.DIR4.UP: v = Vector3.up; break; // 위로 1단위 이동.
			case Tile.DIR4.DOWN: v = Vector3.down; break; // 아래로 1단위 이동.
		}
		v *= Tile.COLLISION_SIZE; // 블록 크기를 곱한다.
		return (v);
	}

	public static Tile.DIR4 getOppositDir(Tile.DIR4 dir)
	{
		Tile.DIR4 opposit = dir;
		switch (dir)
		{
			case Tile.DIR4.RIGHT: opposit = Tile.DIR4.LEFT; break;
			case Tile.DIR4.LEFT: opposit = Tile.DIR4.RIGHT; break;
			case Tile.DIR4.UP: opposit = Tile.DIR4.DOWN; break;
			case Tile.DIR4.DOWN: opposit = Tile.DIR4.UP; break;
		}
		return (opposit);
	}



	public void swapTile(TileControl tile0, Tile.DIR4 dir, TileControl tile1)
	{
		// 각각의 블록 색을 기억해 둔다.
		Tile.COLOR color0 = tile0.color;
		//Tile.COLOR color1 = tile1.color;
		
		// 각각의 블록의.확대율을 기억해 둔다.
		Vector3 scale0 = tile0.transform.localScale;
		Vector3 scale1 = tile1.transform.localScale;
		
		// 각각의 블록의 사라지는 시간을 기억해 둔다.
		float vanish_timer0 = tile0.vanish_timer;
		float vanish_timer1 = tile1.vanish_timer;
		
		// 각각의 블록의 이동할 곳을 구한다.
		Vector3 offset0 = GamePlay.getDirVector(dir);
		Vector3 offset1 = GamePlay.getDirVector(GamePlay.getOppositDir(dir));
		
		//tile0.setColor(color1); // 색을 교체한다.
		tile1.setColor(color0);
		tile0.transform.localScale = scale1; // 확대율을 교체한다.
		tile1.transform.localScale = scale0;
		tile0.vanish_timer = vanish_timer1; // '사라지는 시간'을 교체한다.
		tile1.vanish_timer = vanish_timer0;
		tile0.beginSlide(offset0); // 원래 블록 이동을 시작.
		tile1.beginSlide(offset1); // 이동할 위치의 블록 이동을 시작.
	}


}
