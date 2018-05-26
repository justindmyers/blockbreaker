using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBlocksController : MonoBehaviour {
	public GameObject explosion;
	public AudioClip blockHitSound;

	public AudioClip blockDestroyedSound;

	public AudioSource audioSource;

	public Material CrackMaterial;

	public Block largeBlock = new Block {
		health = 4, 
		points = 40
	};
	
	public Block mediumBlock = new Block {
		health = 2, 
		points = 20
	};

	public Block smallBlock = new Block {
		health = 1, 
		points = 10
	};

	private enum BlockType { Large, Medium, Small };

	private Tilemap tilemap;
	private BoundsInt bounds;
	private TileBase[] collisionTiles;

	private int activeTileCount = 0;

	private LevelManager levelManager;
    private List<Block> blockList = new List<Block>();

	void Start () {
		tilemap = gameObject.GetComponent<Tilemap>();
		activeTileCount = CountActiveTiles();
		levelManager = GameObject.FindObjectOfType<LevelManager>();

		InitBlockList();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D coll) {
        Vector3 hitPosition = Vector3.zero;

        if (tilemap != null)
        {
            foreach (ContactPoint2D hit in coll.contacts)
            {
                hitPosition.x = hit.point.x + (0.5f * hit.normal.x);
                hitPosition.y = hit.point.y + (0.5f * hit.normal.y);

				Vector2 roundedNormal = new Vector2((float)Math.Round(hit.normal.x), (float)Math.Round(hit.normal.y));
				Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);
				TileBase tileHit = tilemap.GetTile(tilePosition);
				Vector3Int blockToDestroyPosition = tilePosition;
				Block blockToDestroy = null;

				Debug.Log(hit.point.x + " " + hit.normal.x +  " " + tilePosition);

				// Go ahead and cache the surrounding blocks
				Vector3Int topTile = new Vector3Int(tilePosition.x, tilePosition.y + 1, tilePosition.z);
				Vector3Int bottomTile = new Vector3Int(tilePosition.x - 1, tilePosition.y + 1, tilePosition.z);
				Vector3Int leftTile = new Vector3Int(tilePosition.x - 1, tilePosition.y, tilePosition.z);
				Vector3Int rightTile = new Vector3Int(tilePosition.x + 1, tilePosition.y, tilePosition.z);

	 			if(tileHit == null) 
				{
					if(roundedNormal == Vector2.right) 
					{
						blockToDestroyPosition = topTile;
					}

					if(roundedNormal == Vector2.left) 
					{
						if(tilemap.GetTile(leftTile) == null) 
						{
							blockToDestroyPosition = bottomTile;
						} 
						else 
						{
							blockToDestroyPosition = leftTile;
						}
					}

					if(roundedNormal == Vector2.up) 
					{
						// If left and right tiles are null, we know we're in a 4 block situation
						if(tilemap.GetTile(leftTile) == null && tilemap.GetTile(rightTile) == null) 
						{
							if(tilemap.GetTile(topTile) == null) 
							{
								blockToDestroyPosition = bottomTile;
							} 
							else 
							{
								blockToDestroyPosition = topTile;
							}			
						} 
						else 
						{
							blockToDestroyPosition = leftTile;
						}
					}

					if(roundedNormal == Vector2.down) {
						blockToDestroyPosition = leftTile;
					}
				}

				audioSource.PlayOneShot(blockHitSound, 0.3f);
				blockToDestroy = GetBlock(blockToDestroyPosition);

				Debug.Log(blockToDestroyPosition);

				if(blockToDestroy != null) {
					if(blockToDestroy.health > 1) {
						blockToDestroy.health--;
					} else {
						levelManager.BlockDestroyed(blockToDestroy);
						tilemap.SetTile(blockToDestroyPosition, null);
						Instantiate(explosion, new Vector3(blockToDestroyPosition.x + 0.5f, blockToDestroyPosition.y + 0.5f, 0), Quaternion.identity);
						audioSource.PlayOneShot(blockDestroyedSound);
					}
				}
            }

			activeTileCount = CountActiveTiles();

			if(activeTileCount == 0) {
				levelManager.GoToNextLevel();
			}
        }
    }

	private int CountActiveTiles() {
		int tileCount = 0;
		
		bounds = tilemap.cellBounds;
		collisionTiles = tilemap.GetTilesBlock(bounds);

		for (int x = 0; x < bounds.size.x; x++) {
			for (int y = 0; y < bounds.size.y; y++) {
				Tile tile = (Tile)collisionTiles[x + y * bounds.size.x];
				if (tile != null) {
					tileCount++;
				}
			}
		}

		return tileCount;
	}

	private Block GetBlock(Vector3Int position) {
		return blockList.Find(x => x.getPosition() == position);
	}

	private void InitBlockList() {
	
		bounds = tilemap.cellBounds;
		collisionTiles = tilemap.GetTilesBlock(bounds);

		for (int x = 0; x < bounds.size.x; x++) {
			for (int y = 0; y < bounds.size.y; y++) {
				Tile tile = (Tile)collisionTiles[x + y * bounds.size.x];

				if (tile != null) {
					/*
					GameObject objToSpawn;

					objToSpawn = new GameObject("Cool GameObject made from Code");

					//Add Components
					SpriteRenderer test = objToSpawn.AddComponent<SpriteRenderer>();

					objToSpawn.transform.position = new Vector3(x + 0.5f, y + 0.5f, -6.0f);
					objToSpawn.transform.localScale = new Vector3(2, 2, 1);

					test.sprite = tile.sprite;
					test.material = CrackMaterial;
 					*/

					// Large Block
					if(tile.sprite.rect.width == 32.00 && tile.sprite.rect.height == 32.00) {
						blockList.Add(new Block {
							tile = tile,
							position = new Vector3Int(x, y, 0),
							health = largeBlock.health,
							points = largeBlock.points
						});
					}

					// Medium Block
					if(tile.sprite.rect.width == 32.00 && tile.sprite.rect.height == 16.00) {
						blockList.Add(new Block {
							tile = tile,
							position = new Vector3Int(x, y, 0),
							health = mediumBlock.health,
							points = mediumBlock.points
						});
					}

					// Large Block
					if(tile.sprite.rect.width == 16.00 && tile.sprite.rect.height == 16.00) {
						blockList.Add(new Block {
							tile = tile,
							position = new Vector3Int(x, y, 0),
							health = smallBlock.health,
							points = smallBlock.points
						});
					}

					Debug.Log(x + " " + y);
				}
			}
		}
	}
}
