import { useContext } from "react";
import List from "../List/List";
import { WorkspaceContext } from "../Side/WorkspaceContext";

const Board = () => {
  const { board } = useContext(WorkspaceContext);

  return (
    <div>
      <div>
        <header>
          <h2>
          {board && board.title ? board.title : ''}
          </h2>
        </header>
      </div>

      <div>
        <List />
      </div>
    </div>
  );
};

export default Board;
