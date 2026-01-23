import { ReactNode } from "react";

interface Column {
  Header: string;
  accessor: string;
  Cell?: (props: { value: unknown; row: { original: unknown } }) => ReactNode;
}

interface TableContainerProps {
  columns: Column[];
  data: Record<string, unknown>[];
  isGlobalFilter?: boolean;
  customPageSize?: number;
  tableClass?: string;
  theadClass?: string;
  trClass?: string;
  thClass?: string;
  divClass?: string;
  SearchPlaceholder?: string;
}

export default function TableContainer({
  columns,
  data,
  tableClass = "",
  theadClass = "",
  trClass = "",
  thClass = "",
  divClass = "",
}: TableContainerProps) {
  return (
    <div className={divClass}>
      <table className={`table align-middle table-nowrap mb-0 ${tableClass}`}>
        <thead className={theadClass}>
          <tr className={trClass}>
            {columns.map((column, index) => (
              <th key={index} scope="col" className={thClass}>
                {column.Header}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {data.map((row, rowIndex) => (
            <tr key={rowIndex}>
              {columns.map((column, colIndex) => (
                <td key={colIndex}>
                  {column.Cell
                    ? column.Cell({ value: row[column.accessor], row: { original: row } })
                    : String(row[column.accessor] ?? "")}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
