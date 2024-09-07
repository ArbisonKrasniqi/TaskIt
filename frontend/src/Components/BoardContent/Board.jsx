import { useContext } from "react";
import { WorkspaceContext } from "../Side/WorkspaceContext";
import ListForm from "../List/ListForm.jsx";
import {DndContext, closestCorners} from "@dnd-kit/core";
import List from "../List/List.jsx";
import {SortableContext, horizontalListSortingStrategy, arrayMove} from "@dnd-kit/sortable";
import { putData } from "../../Services/FetchService.jsx";
import { useParams } from "react-router-dom";

const Board = () => {
  const workspaceContext = useContext(WorkspaceContext);
  const {boardId} = useParams();
  const getListPos = id => workspaceContext.lists.findIndex(list => list.listId === id);

  const updateListBackend = async (originalPos, newPos) => {
    try {
      const data = {
        boardId: boardId,
        oldIndex: originalPos,
        newIndex: newPos
      }
      const response = await putData('/backend/list/DragNDropList', data);
      console.log(response.data);
    } catch (error) {
      console.log(error.message);
    }
  }

  const handleDragEnd = event => {
    const {active, over} = event;
    if (active.id === over.id) return;

    try {
      
      workspaceContext.setLists(list => {
        const originalPos = getListPos(active.id);
        const newPos = getListPos(over.id);

        console.log("OriginalPosition: "+originalPos+" moved to new position: "+newPos);
        updateListBackend(originalPos, newPos);
        return arrayMove(list, originalPos, newPos);
      })
    } catch (error) {
    console.log("Error")
    }
  };

  return (
    <div>
      <div>
        <header>
          <h2>
          {workspaceContext.board && workspaceContext.board.title ? workspaceContext.board.title : ''}
          </h2>
        </header>
      </div>
      
      <div className="p-10 bg-gray-200 min-h-screen">
      <DndContext onDragEnd={handleDragEnd} collisionDetection={closestCorners}>
          <div className="flex space-x-4">
            <SortableContext items={workspaceContext.lists.map(list => list.listId)} strategy={horizontalListSortingStrategy}>
              {workspaceContext.lists.map((list) => (
                <List key={list.listId} list={list} />
              ))}
            </SortableContext>
            <ListForm />
          </div>
        </DndContext>
      </div>
    </div>
  );
};

export default Board;
