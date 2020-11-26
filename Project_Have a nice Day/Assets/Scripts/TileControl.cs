using UnityEngine;
using System.Collections;


// 블록에 관한 정보를 다룬다.
public class Tile
{
	public static float COLLISION_SIZE = 1.0f; // 블록의 충돌 크기.
	public static float VANISH_TIME = 3.0f;
	public struct iPosition
	{
		public int x;
		public int y;
	}
	public enum COLOR
	{
		NONE = -1,
		RED = 0,
		BLUE,
		YELLOW,
		GREEN,
		BLACK,
		NUM,
		NORMAL_COLOR_NUM = BLACK, // 보통 색의 수.
	};

	public enum DIR4
	{
		NONE = -1, // 방향 지정 없음.
		RIGHT,
		LEFT,
		UP,
		DOWN,
		NUM,
	};

	public enum STEP
	{ // 블록의 상태를 나타낸다.
		NONE = -1, // 상태 정보 없음.
		IDLE = 0,  // 대기중.
		GRABBED,   // 잡혀 있음.
		RELEASED,
		SLIDE,
		VACANT,
		NUM,
	};

	// 몇 x 몇 설정
	public static int TILE_NUM_X = 5;
	public static int TILE_NUM_Y = 5;
}



public class TileControl : MonoBehaviour
{
	public Tile.COLOR color = (Tile.COLOR)0;
	public GamePlay tile_root = null;
	public Tile.iPosition i_pos;

	public Tile.STEP step = Tile.STEP.NONE;         // 지금 상태
	public Tile.STEP next_step = Tile.STEP.NONE;    // 다음 상태
	private Vector3 position_offset_initial = Vector3.zero; // 교체 전 위치
	public Vector3 position_offset = Vector3.zero;          // 교체 후 위치


	public float vanish_timer = -1.0f;                  // 블록이 사라질 때까지의 시간
	public Tile.DIR4 slide_dir = Tile.DIR4.NONE;        // 슬라이드된 방향
	public float step_timer = 0.0f;                     // 블록이 교체된 때의 이동시간


	void Start()
	{
		this.setColor(this.color); // 색칠

		this.next_step = Tile.STEP.IDLE; // 다음 블록을 대기중으로
	}

	void Update()
	{
		Vector3 mouse_position; // 마우스 위치.
		this.tile_root.unprojectMousePosition(out mouse_position, Input.mousePosition);
		// 획득한 마우스 위치를 X와 Y만으로 한다.
		Vector2 mouse_position_xy = new Vector2(mouse_position.x, mouse_position.y);

		this.step_timer += Time.deltaTime;
		float slide_time = 0.2f;
		if (this.next_step == Tile.STEP.NONE)
		{ // '상태정보 없음'의 경우.
			switch (this.step)
			{
				case Tile.STEP.SLIDE:
					if (this.step_timer >= slide_time)
					{
						// vanish_timer(사라질 때까지의 시간)이 0이면 VACANT(사라지는)상태로 이행.
						if (this.vanish_timer == 0.0f)
						{
							this.next_step = Tile.STEP.VACANT;
							// vanish_timer가 0이 아니면 IDLE(대기) 상태로 이행.
						}
						else
						{
							this.next_step = Tile.STEP.IDLE;
						}
					}
					break;
			}
		}



		// 다음 블록 상태가 정보 없음 이외인 동안 -> 다음 블록 상태가 변경된 경우
		while (this.next_step != Tile.STEP.NONE)
		{
			this.step = this.next_step;
			this.next_step = Tile.STEP.NONE;
			switch (this.step)
			{
				case Tile.STEP.IDLE: // 대기 상태
					this.position_offset = Vector3.zero;
					this.transform.localScale = Vector3.one * 1.0f;
					break;
				case Tile.STEP.GRABBED: // 잡힌 상태
					this.transform.localScale = Vector3.one * 1.2f;
					break;
				case Tile.STEP.RELEASED: // 떨어져 있는 상태
					this.position_offset = Vector3.zero;
					this.transform.localScale = Vector3.one * 1.0f;
					break;

				case Tile.STEP.VACANT:
					this.position_offset = Vector3.zero;
					break;
			}
			this.step_timer = 0.0f;
		}


		switch (this.step)
		{
			case Tile.STEP.GRABBED: // 잡힌 상태.
									// 잡힌 상태일 때는 항상 슬라이드 방향을 체크.
				this.slide_dir = this.calcSlideDir(mouse_position_xy);
				break;
			case Tile.STEP.SLIDE: // 슬라이드(교체) 중.
								  // 블록을 서서히 이동하는 처리.
				float rate = this.step_timer / slide_time;
				rate = Mathf.Min(rate, 1.0f);
				rate = Mathf.Sin(rate * Mathf.PI / 2.0f);
				this.position_offset = Vector3.Lerp(this.position_offset_initial, Vector3.zero, rate);
				break;
		}


		//그리드 좌표를 실제 좌표로 변환
		Vector3 position = GamePlay.calcTilePosition(this.i_pos) + this.position_offset;
		// 실제 위치를 새로운 위치로 변경.
		this.transform.position = position;
	}


	// 인수 color의 색으로 블록을 칠한다.
	public void setColor(Tile.COLOR color)
	{
		this.color = color; // 이번에 지정된 색을 멤버 변수에 보관.
		Color color_value; // Color클래스는 색을 나타낸다. 
		switch (this.color)
		{
			default:
			case Tile.COLOR.RED:
				color_value = Color.red;
				break;
			case Tile.COLOR.BLUE:
				color_value = Color.blue;
				break;
			case Tile.COLOR.YELLOW:
				color_value = Color.yellow;
				break;
			case Tile.COLOR.GREEN:
				color_value = Color.green;
				break;
			case Tile.COLOR.BLACK:
				color_value = Color.black;
				break;
		}
		this.GetComponent<Renderer>().material.color = color_value;
	}


	public void beginGrab()
	{
		this.next_step = Tile.STEP.GRABBED;
	}

	public void endGrab()
	{
		this.next_step = Tile.STEP.IDLE;
	}

	public bool isGrabbable()
	{
		bool is_grabbable = false;
		switch (this.step)
		{
			case Tile.STEP.IDLE: // '대기' 상태일 때만.
				is_grabbable = true; // true(잡을 수 있다)를 반환한다.
				break;
		}
		return (is_grabbable);
	}

	public bool isContainedPosition(Vector2 position)
	{
		bool ret = false;
		Vector3 center = this.transform.position;
		float h = Tile.COLLISION_SIZE / 2.0f;
		do
		{
			if (position.x < center.x - h || center.x + h < position.x)
			{
				break;
			}
			if (position.y < center.y - h || center.y + h < position.y)
			{
				break;
			}
			ret = true; //겹쳐있다를 반환
		} while (false);
		return (ret);
	}


	public Tile.DIR4 calcSlideDir(Vector2 mouse_position)
	{
		Tile.DIR4 dir = Tile.DIR4.NONE;
		// 지정된 mouse_position과 현재 위치의 차를 나타내는 벡터.
		Vector2 v = mouse_position -
			new Vector2(this.transform.position.x, this.transform.position.y);
		// 벡터의 크기가 0.1보다 크면.
		// (그보다 작으면 슬라이드 하지 않은 걸로 간주한다).
		if (v.magnitude > 0.1f)
		{
			if (v.y > v.x)
			{
				if (v.y > -v.x)
				{
					dir = Tile.DIR4.UP;
				}
				else
				{
					dir = Tile.DIR4.LEFT;
				}
			}
			else
			{
				if (v.y > -v.x)
				{
					dir = Tile.DIR4.RIGHT;
				}
				else
				{
					dir = Tile.DIR4.DOWN;
				}
			}
		}
		return (dir);
	}

	public float calcDirOffset(Vector2 position, Tile.DIR4 dir)
	{
		float offset = 0.0f;
		// 지정된 위치와 블록의 현재 위치의 차를 나타내는 벡터.
		Vector2 v = position - new Vector2(
			this.transform.position.x, this.transform.position.y);
		switch (dir)
		{ // 지정된 방향에 따라 갈라진다. 
			case Tile.DIR4.RIGHT:
				offset = v.x;
				break;
			case Tile.DIR4.LEFT:
				offset = -v.x;
				break;
			case Tile.DIR4.UP:
				offset = v.y;
				break;
			case Tile.DIR4.DOWN:
				offset = -v.y;
				break;
		}
		return (offset);
	}

	public void beginSlide(Vector3 offset)
	{
		this.position_offset_initial = offset;
		this.position_offset =
			this.position_offset_initial;
		// 상태를 SLIDE로 변경.
		this.next_step = Tile.STEP.SLIDE;
	}

}
